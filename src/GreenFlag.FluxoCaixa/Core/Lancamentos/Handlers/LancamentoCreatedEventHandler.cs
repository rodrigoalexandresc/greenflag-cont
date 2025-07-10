using GreenFlag.Commons.Messaging;
using GreenFlag.Financeiro.Api.Data;
using GreenFlag.Financeiro.Core.Lancamentos.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GreenFlag.Financeiro.Core.Lancamentos.Handlers
{
    public class LancamentoCreatedEventHandler : INotificationHandler<LancamentoCreatedEvent>
    {
        private readonly FinanceiroDbContext _context;
        private readonly ILogger<LancamentoCreatedEventHandler> _logger;
        private readonly IMessageProducer _messageProducer;

        public LancamentoCreatedEventHandler(FinanceiroDbContext context, ILogger<LancamentoCreatedEventHandler> logger, IMessageProducer messageProducer)
        {
            _context = context;
            _logger = logger;
            _messageProducer = messageProducer;
        }

        public async Task Handle(LancamentoCreatedEvent notification, CancellationToken cancellationToken)
        {
            var lancamento = await _context.Lancamentos.FirstOrDefaultAsync(l => l.Id == notification.LancamentoId);
            var lancamentoMessage = new LancamentoMessage
            {
                Categoria = lancamento.Categoria,
                DataLancamento = lancamento.DataLancamento,
                Tipo = lancamento.Tipo,
                Valor = lancamento.Valor
            };

            await _messageProducer.ProduceAsync(lancamentoMessage, "lancamento-created");           
        }
    }
}
