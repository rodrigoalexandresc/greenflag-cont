using GreenFlag.Commons.Messaging;
using GreenFlag.ConsolidaDiario.Core.Consolidacao.Commands;
using GreenFlag.ConsolidaDiario.Core.Consolidacao.Handlers;
using GreenFlag.ConsolidaDiario.Core.Consolidacao.Queries;
using GreenFlag.ConsolidaDiario.Data;
using MediatR;

namespace GreenFlag.ConsolidaDiario
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddMediatR(typeof(ConsolidacaoQueryHandler).Assembly);

            builder.Services.AddSingleton<ConsolidacaoDbContext>();
            builder.Services.AddSingleton<IMessageProducer, KafkaMessageProducer>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/relatorio", async(DateTime? dataInicio, DateTime? dataFim, IMediator _mediator) =>
            {
                var response = await _mediator.Send(new ConsolidacaoQueryRequest(dataInicio, dataFim));
                return Results.Ok(response);    
            });

            app.MapPost("consolidar-dia", async (ConsolidarDiaRequest request, IMediator _mediator) =>
            {
                var res = await _mediator.Send(request);
                return Results.Ok(res);
            });

            app.Run();
        }
    }
}
