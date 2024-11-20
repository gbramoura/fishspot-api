using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FishspotApi.Domain.Entity
{
    public class RecoverPasswordEntity : BaseEntity
    {
        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        public string Email { get; set; }

        [BsonElement("token"), BsonRepresentation(BsonType.String)]
        public string Token { get; set; }

        [BsonElement("expiration_date"), BsonRepresentation(BsonType.DateTime)]
        public DateTime ExpirationDate { get; set; }
    }
}