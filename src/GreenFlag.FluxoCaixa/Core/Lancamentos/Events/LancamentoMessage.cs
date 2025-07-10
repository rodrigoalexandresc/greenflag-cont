namespace GreenFlag.Financeiro.Core.Lancamentos.Events
{
    public sealed record LancamentoMessage
    {
        public DateTime DataLancamento { get; set; }
        public string Categoria { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
    }
}
