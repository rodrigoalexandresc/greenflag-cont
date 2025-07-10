namespace GreenFlag.Financeiro.Core.Lancamentos.Entities
{
    public class Lancamento
    {
        public Guid Id { get; set; }
        public DateTime DataLancamento { get; set; }
        public string Categoria { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public string Pessoa { get; set; }
        public string DocumentoPessoa { get; set; }

    }
}
