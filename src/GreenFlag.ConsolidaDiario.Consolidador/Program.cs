using GreenFlag.ConsolidaDiario.Consolidador;
using Quartz;

namespace GreenFlag.ConsolidaDiario.Scheduler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddQuartz(q =>
            {
                var jobKey = new JobKey("ConsolidadorJob");
                q.AddJob<ConsolidadorJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(options =>
                {
                    options.ForJob(jobKey)
                    .WithIdentity("ConsolidadorJobTrigger")
                    .WithCronSchedule("0 0 0 * * ?");
                });
            });

            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            var host = builder.Build();
            host.Run();
        }
    }
}