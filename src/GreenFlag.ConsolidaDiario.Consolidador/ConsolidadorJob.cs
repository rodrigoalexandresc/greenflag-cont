using Confluent.Kafka;
using GreenFlag.ConsolidaDiario.Core.Consolidacao.Messages;
using Quartz;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GreenFlag.ConsolidaDiario.Consolidador
{
    public class ConsolidadorJob : IJob
    {    
        public async Task Execute(IJobExecutionContext context)
        {
            var dataCalculo = context.FireTimeUtc.UtcDateTime;
            dataCalculo.AddDays(-1);

            var diaCalculo = context.FireTimeUtc.AddDays(-1);

            var json = JsonSerializer.Serialize(new ConsolidacaoDiariaMessage(dataCalculo), new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            await producer.ProduceAsync(
                "consolidacao-diaria-start",
                new Message<Null, string> { Value = json });
        }
    }
}
