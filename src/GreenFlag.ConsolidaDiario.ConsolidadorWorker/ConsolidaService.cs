using GreenFlag.ConsolidaDiario.ConsolidadorWorker;
using GreenFlag.ConsolidaDiario.Core.Consolidacao.Entities;
using GreenFlag.ConsolidaDiario.Data;
using GreenFlag.ConsolidaDiario.Data.Collections;
using MongoDB.Driver;

namespace GreenFlag.ConsolidaDiario.Core.Lancamentos
{
    public class ConsolidaService : IConsolidaService
    {
        private readonly ConsolidacaoDbContext _consolidacaoDbContext;

        public ConsolidaService(ConsolidacaoDbContext consolidacaoDbContext)
        {
            _consolidacaoDbContext = consolidacaoDbContext;
        }

        public async Task ConsolidarLancamentosDia(DateTime dia, CancellationToken cancellationToken)
        {
            var lancamentosResumido = _consolidacaoDbContext.GetLancamentoResumidoCollection();

            var diaCalculo = DateTime.UtcNow.Date;

            var lancamentosDoDia = (await lancamentosResumido
                .FindAsync(x => x.DataLancamento.Date == diaCalculo, null, cancellationToken))
                .ToList();

            var diaConsolidadoCollection = _consolidacaoDbContext.GetDiaConsolidadoCollection();
            var saldoDiaAnterior = diaConsolidadoCollection.Find(x => x.Dia == diaCalculo.AddDays(-1)).FirstOrDefault()?.SaldoFinal ?? 0;

            var diaConsolidadoCalculo = new DiaConsolidado
            {
                Dia = diaCalculo,
                SaldoDiaAnterior = saldoDiaAnterior,
                Entradas = lancamentosDoDia.Where(x => x.Tipo == "Entrada").Sum(x => x.Valor),
                Saidas = lancamentosDoDia.Where(x => x.Tipo == "Saida").Sum(x => x.Valor),
                SaldoFinal = saldoDiaAnterior + lancamentosDoDia.Sum(x => x.Tipo == "Entrada" ? x.Valor : -x.Valor),
                Categorias = lancamentosDoDia
                    .GroupBy(x => x.Categoria)
                    .Select(g => new DiaConsolidadoCategoria
                    {
                        Categoria = g.Key,
                        SaldoDia = g.Sum(x => x.Tipo == "Entrada" ? x.Valor : -x.Valor)
                    })
                    .ToList()
            };

            await diaConsolidadoCollection.InsertOneAsync(diaConsolidadoCalculo, cancellationToken: cancellationToken);
        }
    }
}
