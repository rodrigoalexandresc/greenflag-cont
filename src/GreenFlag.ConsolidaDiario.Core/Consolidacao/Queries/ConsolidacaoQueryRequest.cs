using GreenFlag.ConsolidaDiario.Core.Consolidacao.Responses;
using MediatR;

namespace GreenFlag.ConsolidaDiario.Core.Consolidacao.Queries
{
    public record ConsolidacaoQueryRequest(DateTime? DataInicio, DateTime? DataFim) : IRequest<IEnumerable<ConsolidacaoQueryResponse>>
    {
    }
}
