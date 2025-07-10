namespace GreenFlag.ConsolidaDiario.Core.Consolidacao.Responses
{
    public record ConsolidacaoQueryResponse
    {
        public DateTime Dia { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal Entradas { get; set; }
        public decimal Saidas { get; set; }
        public decimal SaldoFinal { get; set; }
    }
}
