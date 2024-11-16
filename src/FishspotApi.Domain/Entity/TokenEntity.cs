using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FishspotApi.Domain.Entity
{
    public class TokenEntity : BaseEntity
    {
        [BsonElement("actor"), BsonRepresentation(BsonType.String)]
        public string Actor { get; set; }

        [BsonElement("refresh_token"), BsonRepresentation(BsonType.String)]
        public string RefreshToken { get; set; }

        [BsonElement("token"), BsonRepresentation(BsonType.String)]
        public string Token { get; set; }

    }
}
