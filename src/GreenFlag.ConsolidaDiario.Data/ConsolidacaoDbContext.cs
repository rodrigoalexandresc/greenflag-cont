using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace GreenFlag.ConsolidaDiario.Data
{
    public class ConsolidacaoDbContext
    {
        public IMongoDatabase Database { get; }

        public ConsolidacaoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDb:ConnectionString"];
            var databaseName = configuration["MongoDb:Database"];
            var client = new MongoClient(connectionString);
            Database = client.GetDatabase(databaseName);
        }
    }
}
