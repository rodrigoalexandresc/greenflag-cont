using GreenFlag.ConsolidaDiario.Core.Lancamentos.Entities;
using MongoDB.Driver;

namespace GreenFlag.ConsolidaDiario.Data.Collections
{
    public static class LancamentoDbConnectionExtensions
    {
        public static IMongoCollection<LancamentoResumido> GetLancamentoResumidoCollection(this ConsolidacaoDbContext dbConnection) =>  
            dbConnection.Database.GetCollection<LancamentoResumido>("lancamento-resumido");
    }
}
