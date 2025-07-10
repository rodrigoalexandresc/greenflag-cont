using GreenFlag.ConsolidaDiario.Core.Consolidacao.Responses;
using MediatR;

namespace GreenFlag.ConsolidaDiario.Core.Consolidacao.Commands
{
    public record ConsolidarDiaRequest : IRequest<ConsolidarDiaResponse>
    {
        public DateTime DataParaConsolidacao { get; set; }
    }
}
