using Confluent.Kafka;
using GreenFlag.Commons.Messaging;
using GreenFlag.ConsolidaDiario.Core.Consolidacao.Messages;
using System.Text.Json;

namespace GreenFlag.ConsolidaDiario.ConsolidadorWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        //private readonly ConsumerConfig _consumerConfig;
        private readonly IConsolidaService _consolidaService;

        private readonly IMessageConsumer _messageConsumer;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IConsolidaService consolidaService, IMessageConsumer messageConsumer)
        {
            _logger = logger;
            //_consumerConfig = new ConsumerConfig
            //{
            //    BootstrapServers = "localhost:9092",
            //    GroupId = "consolidacao-diaria-start-group",
            //    AutoOffsetReset = AutoOffsetReset.Earliest,
            //    EnableAutoCommit = false,
            //};
            _consolidaService = consolidaService;
            _messageConsumer = messageConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageConsumer.Consume("consolidacao-diaria-start", async (ConsolidacaoDiariaMessage payload, CancellationToken ct) =>
            {
                await _consolidaService.ConsolidarLancamentosDia(payload.dataConsolidacao.Date, ct);
            }, stoppingToken);

            //using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
            //consumer.Subscribe("consolidacao-diaria-start");

            //_logger.LogInformation("Aguardando mensagens no tópico 'consolidacao-diaria-start'...");

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    try
            //    {
            //        var result = consumer.Consume(stoppingToken);

            //        _logger.LogInformation("Mensagem recebida: {Message}, Tópico: {Topic}, Partição: {Partition}, Offset: {Offset}",
            //            result.Message.Value, result.Topic, result.Partition, result.Offset);

            //        var payload = JsonSerializer.Deserialize<ConsolidacaoDiariaMessage>(result.Message.Value);

            //        _logger.LogInformation("Consolidação solicitada para {DataLancamento}", payload.dataConsolidacao.Date);

            //        await _consolidaService.ConsolidarLancamentosDia(payload.dataConsolidacao.Date, stoppingToken);

            //        consumer.Commit();
            //    }
            //    catch (ConsumeException ex)
            //    {
            //        _logger.LogError($"Erro no Kafka: {ex.Error.Reason}");
            //    }

            //    await Task.Delay(100, stoppingToken);
            //}

            //consumer.Close();
        }
    }
}