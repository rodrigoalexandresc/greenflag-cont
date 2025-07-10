using GreenFlag.Financeiro.Api.Data;
using GreenFlag.Financeiro.Core.Lancamentos.Entities;
using GreenFlag.Financeiro.Core.Lancamentos.Events;
using GreenFlag.Financeiro.Core.Lancamentos.Requests;
using GreenFlag.Financeiro.Core.Lancamentos.Responses;
using MediatR;

namespace GreenFlag.Financeiro.Core.Lancamentos.Handlers
{
    public class LancamentoCreateCommandHandler : IRequestHandler<LancamentoCreateRequest, LancamentoCreateResponse>
    {
        private readonly FinanceiroDbContext _dbContext;
        private readonly IMediator _mediator;

        public LancamentoCreateCommandHandler(FinanceiroDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<LancamentoCreateResponse> Handle(LancamentoCreateRequest request, CancellationToken cancellationToken)
        {
            var lancamento = new Lancamento
            {
                Id = Guid.NewGuid(),
                DataLancamento = request.DataLancamento,
                Categoria = request.Categoria,
                Descricao = request.Descricao,
                Tipo = request.Tipo,
                Valor = request.Valor,
                Pessoa = request.Pessoa,
                DocumentoPessoa = request.DocumentoPessoa
            };

            _dbContext.Add(lancamento);

            await _dbContext.SaveChangesAsync();

            await _mediator.Publish(new LancamentoCreatedEvent
            {
                LancamentoId = lancamento.Id
            });
            
            return new LancamentoCreateResponse { Id = lancamento.Id }; 
        }
    }
}
