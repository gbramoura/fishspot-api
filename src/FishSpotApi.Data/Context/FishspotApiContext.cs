using FishSpotApi.Logger;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FishSpotApi.Data.Context;

public class FishSpotApiContext
{
    public readonly IMongoDatabase? Database;

    public FishSpotApiContext(IConfiguration config)
    {
        Database = StartDatabase(config);
    }

    private static IMongoDatabase StartDatabase(IConfiguration config)
    {
        try
        {
            LoggerFactory.Info("Start the database connection");

            var dbConnection = config["MongoDB:ConnectionURI"];
            var mongoUrl = MongoUrl.Create(dbConnection);
            var mongoClient = new MongoClient(mongoUrl);

            var database = mongoClient.GetDatabase(mongoUrl.DatabaseName);

            database.RunCommand<BsonDocument>(new BsonDocument("ping", 1));

            LoggerFactory.Info("Connection successful!");
            return database;
        }
        catch (Exception ex)
        {
            LoggerFactory.Error("Connection failed", ex);
            throw;
        }
    }
}