namespace GreenFlag.ConsolidaDiario.Worker.Core
{
    public record LancamentoMessage
    {
        public DateTime dataLancamento { get; set; }
        public string categoria { get; set; }
        public string tipo { get; set; }
        public decimal valor { get; set; }
    }
}
