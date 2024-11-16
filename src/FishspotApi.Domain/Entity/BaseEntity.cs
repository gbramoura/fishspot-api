using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FishspotApi.Domain.Entity
{
    public class BaseEntity
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
