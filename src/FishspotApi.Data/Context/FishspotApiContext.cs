using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace FishspotApi.Data.Context
{
    public class FishspotApiContext
    {
        private readonly IMongoDatabase? _database;

        public FishspotApiContext(IConfiguration config)
        {
            var dbConnection = config["MongoDB:ConnectionURI"];
            var mongoUrl = MongoUrl.Create(dbConnection);
            var mongoClient = new MongoClient(mongoUrl);
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        public IMongoDatabase? Database => _database;
    }
}