using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GreenFlag.Commons.Messaging
{
    public class KafkaMessageConsumer : IMessageConsumer
    {
        private readonly ILogger<KafkaMessageConsumer> _logger;
        private readonly ConsumerConfig _consumerConfig;        

        public KafkaMessageConsumer(ILogger<KafkaMessageConsumer> logger, IConfiguration configuration)
        {
            _logger = logger;
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "consolidacao-diaria-start-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
            };
        }

        public async Task Consume<T>(string topicName, Func<T, CancellationToken, Task> func, CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
            consumer.Subscribe(topicName);

            _logger.LogInformation($"Aguardando mensagens no tópico '{topicName}'...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);

                    _logger.LogInformation("Mensagem recebida: {Message}, Tópico: {Topic}, Partição: {Partition}, Offset: {Offset}",
                        result.Message.Value, result.Topic, result.Partition, result.Offset);

                    var payload = JsonSerializer.Deserialize<T>(result.Message.Value);

                    await func(payload, stoppingToken);

                    _logger.LogInformation("Mensagem processada: {Message}, Tópico: {Topic}, Partição: {Partition}, Offset: {Offset}",
                        result.Message.Value, result.Topic, result.Partition, result.Offset);

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
