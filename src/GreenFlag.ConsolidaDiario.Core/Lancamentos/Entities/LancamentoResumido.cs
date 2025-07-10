using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GreenFlag.ConsolidaDiario.Core.Lancamentos.Entities
{
    public class LancamentoResumido
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime DataLancamento { get; set; }
        public string Categoria { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
    }
}
