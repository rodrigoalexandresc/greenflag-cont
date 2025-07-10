using GreenFlag.Commons.Messaging;
using GreenFlag.Financeiro.Api.Data;
using GreenFlag.Financeiro.Core.Lancamentos.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GreenFlag.FluxoCaixa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<FinanceiroDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("LancamentosDbConnection")));

            builder.Services.AddSingleton<IMessageProducer, KafkaMessageProducer>();

            builder.Services.AddMediatR(typeof(Program));

            builder.Services.AddAuthorization();

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

            app.MapPost("/lancamentos", async (LancamentoCreateRequest request, IMediator mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            });

            app.Run();
        }
    }
}
