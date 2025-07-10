using MediatR;

namespace GreenFlag.Financeiro.Core.Lancamentos.Events
{
    public class LancamentoCreatedEvent : INotification
    {
        public Guid LancamentoId { get; set; }
    }
}
