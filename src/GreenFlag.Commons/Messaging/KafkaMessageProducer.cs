using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace GreenFlag.Commons.Messaging
{
    public class KafkaMessageProducer : IMessageProducer
    {
        private readonly ILogger<KafkaMessageProducer> _logger;
        private readonly IConfiguration _configuration;

        public KafkaMessageProducer(ILogger<KafkaMessageProducer> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task ProduceAsync<T>(T message, string topicName)
        {
            var json = JsonSerializer.Serialize(message, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var config = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"] //localhost:9092"
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var result = await producer.ProduceAsync(
                topicName,
                new Message<Null, string> { Value = json });

            _logger.LogInformation($"Mensagem enviada para o tópico {result.Topic} na partição {result.Partition} com offset {result.Offset}");
        }
    }
}
