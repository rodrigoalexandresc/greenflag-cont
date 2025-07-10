using Confluent.Kafka;
using GreenFlag.ConsolidaDiario.Core.Lancamentos.Entities;
using GreenFlag.ConsolidaDiario.Worker.Core;
using MongoDB.Driver;
using System.Text.Json;

namespace GreenFlag.ConsolidaDiario.RegistroWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ConsumerConfig _consumerConfig;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "consolida-diario-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
            };
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
            consumer.Subscribe("lancamento-created");

            _logger.LogInformation("Aguardando mensagens no tópico 'lancamento-created'...");

            var connectionString = _configuration["MongoDb:ConnectionString"];
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(_configuration["MongoDb:Database"]);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);

                    _logger.LogInformation("Mensagem recebida: {Message}, Tópico: {Topic}, Partição: {Partition}, Offset: {Offset}",
                        result.Message.Value, result.Topic, result.Partition, result.Offset);

                    var payload = JsonSerializer.Deserialize<LancamentoMessage>(result.Message.Value);

                    var collection = database.GetCollection<LancamentoResumido>("lancamento-resumido");

                    var lancamentoResumido = new LancamentoResumido
                    {
                        DataLancamento = payload.dataLancamento.Date,
                        Categoria = payload.categoria,
                        Tipo = payload.tipo,
                        Valor = payload.valor
                    };

                    await collection.InsertOneAsync(lancamentoResumido);

                    consumer.Commit();
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Erro no Kafka: {ex.Error.Reason}");
                }

                await Task.Delay(100, stoppingToken);
            }

            consumer.Close();
        }
    }
}
