using GreenFlag.ConsolidaDiario.Core.Consolidacao.Entities;
using GreenFlag.ConsolidaDiario.Core.Lancamentos.Entities;
using MongoDB.Driver;

namespace GreenFlag.ConsolidaDiario.Data.Collections
{
    public static class ConsolidacaoDbConnectionExtensions
    {
        public static IMongoCollection<DiaConsolidado> GetDiaConsolidadoCollection(this ConsolidacaoDbContext dbContext) =>
            dbContext.Database.GetCollection<DiaConsolidado>("dia-consolidado");
    }
}
