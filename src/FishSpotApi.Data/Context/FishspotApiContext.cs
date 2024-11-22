using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace FishSpotApi.Data.Context;

public class FishSpotApiContext
{
    private readonly IMongoDatabase? _database;

    public FishSpotApiContext(IConfiguration config)
    {
        var dbConnection = config["MongoDB:ConnectionURI"];
        var mongoUrl = MongoUrl.Create(dbConnection);
        var mongoClient = new MongoClient(mongoUrl);
        _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
    }

    public IMongoDatabase? Database => _database;
}