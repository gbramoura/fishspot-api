using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FishspotApi.Domain.Entity
{
    public class UserEntity : BaseEntity
    {
        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string Name { get; set; }

        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        public string Email { get; set; }

        [BsonElement("password"), BsonRepresentation(BsonType.String)]
        public string Password { get; set; }

        [BsonElement("Unique_identifier_token"), BsonRepresentation(BsonType.String)]
        public string UniqueIdentifierToken { get; set; }
    }
}
