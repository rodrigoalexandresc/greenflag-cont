using GreenFlag.Commons.Messaging;
using GreenFlag.ConsolidaDiario.Core.Consolidacao.Commands;
using GreenFlag.ConsolidaDiario.Core.Consolidacao.Messages;
using GreenFlag.ConsolidaDiario.Core.Consolidacao.Responses;
using MediatR;

namespace GreenFlag.ConsolidaDiario.Core.Consolidacao.Handlers
{
    public class ConsolidarDiaCommandHandler : IRequestHandler<ConsolidarDiaRequest, ConsolidarDiaResponse>
    {
        private readonly IMessageProducer _messageProducer;

        public ConsolidarDiaCommandHandler(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

        public async Task<ConsolidarDiaResponse> Handle(ConsolidarDiaRequest request, CancellationToken cancellationToken)
        {
            await _messageProducer.ProduceAsync(new ConsolidacaoDiariaMessage(request.DataParaConsolidacao), "consolidacao-diaria-start");

            return new ConsolidarDiaResponse
            {
            };
        }
    }
}
