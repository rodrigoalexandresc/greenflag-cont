using GreenFlag.Commons.Messaging;
using GreenFlag.ConsolidaDiario.Core.Lancamentos;
using GreenFlag.ConsolidaDiario.Data;

namespace GreenFlag.ConsolidaDiario.ConsolidadorWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton<ConsolidacaoDbContext>();    
            builder.Services.AddSingleton<IConsolidaService, ConsolidaService>();
            builder.Services.AddSingleton<IMessageConsumer, KafkaMessageConsumer>();
            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}