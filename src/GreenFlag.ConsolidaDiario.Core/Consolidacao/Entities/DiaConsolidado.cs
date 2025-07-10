using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace GreenFlag.ConsolidaDiario.Core.Consolidacao.Entities
{
    public class DiaConsolidado
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime Dia { get; set; }
        public decimal SaldoDiaAnterior { get; set; }
        public decimal Entradas { get; set; }
        public decimal Saidas { get; set; }
        public decimal SaldoFinal { get; set; }
        public IEnumerable<DiaConsolidadoCategoria> Categorias { get; set; }
    }

    public class DiaConsolidadoCategoria
    {
        public string Categoria { get; set; }
        public decimal SaldoDia { get; set; }
    }
}
