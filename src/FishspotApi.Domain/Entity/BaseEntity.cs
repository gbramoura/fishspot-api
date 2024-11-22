using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FishSpotApi.Domain.Entity;

public class BaseEntity
{
    [BsonId]
    [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}