using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FishspotApi.Domain.Entity.Objects
{
    public class SpotUser
    {
        [BsonElement("id"), BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string Name { get; set; }
    }
}