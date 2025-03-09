using MongoDB.Bson;

namespace FishSpotApi.Core.Utils;

public static class MongoJsonUtils
{
    public static List<BsonDocument> GetListOfBsonDocuments(BsonDocument document, string arrayFieldName)
    {
        if (!document.Contains(arrayFieldName) || !document[arrayFieldName].IsBsonArray)
        {
            return [];
        }
        
        var bsonArray = document[arrayFieldName].AsBsonArray;
        var documents = new List<BsonDocument>();
        
        bsonArray.ToList().ForEach(item =>
        {
            documents.Add(item.AsBsonDocument);
        });
        
        return documents;
    }
}