using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishspotApi.Data.Context
{
    public class FishspotApiContext
    {
        private readonly IMongoDatabase? _database;

        public FishspotApiContext(string dbConnection)
        {
            var mongoUrl = MongoUrl.Create(dbConnection);
            var mongoClient = new MongoClient(mongoUrl);
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        public IMongoDatabase? Database => _database;
    }
}
