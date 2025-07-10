using GreenFlag.ConsolidaDiario.Core.Consolidacao.Entities;
using GreenFlag.ConsolidaDiario.Core.Consolidacao.Queries;
using GreenFlag.ConsolidaDiario.Core.Consolidacao.Responses;
using GreenFlag.ConsolidaDiario.Data;
using MediatR;
using MongoDB.Driver;

namespace GreenFlag.ConsolidaDiario.Core.Consolidacao.Handlers
{
    public class ConsolidacaoQueryHandler : IRequestHandler<ConsolidacaoQueryRequest, IEnumerable<ConsolidacaoQueryResponse>>
    {
        private readonly ConsolidacaoDbContext _consolidacaoDbContext;

        public ConsolidacaoQueryHandler(ConsolidacaoDbContext consolidacaoDbContext)
        {
            _consolidacaoDbContext = consolidacaoDbContext;
        }

        public async Task<IEnumerable<ConsolidacaoQueryResponse>> Handle(ConsolidacaoQueryRequest request, CancellationToken cancellationToken)
        {
            var collection = _consolidacaoDbContext.Database.GetCollection<DiaConsolidado>("dia-consolidado");

            var result = await collection
                .Find(_ => true)
                .Project(o => new ConsolidacaoQueryResponse
                {
                    Dia = o.Dia,
                    Entradas = o.Entradas,
                    Saidas = o.Saidas,
                    SaldoInicial = o.SaldoDiaAnterior,
                    SaldoFinal = o.SaldoFinal
                })
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
