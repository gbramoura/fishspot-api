using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FishspotApi.Domain.Configuration
{
    public class AuthenticationConfig
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("access_token_secret"), BsonRepresentation(BsonType.String)]
        public string AccessTokenSecret { get; set; }

        [BsonElement("access_token_expirantion_minutes"), BsonRepresentation(BsonType.String)]
        public string AccessTokenExpirantionMinutes { get; set; }

        [BsonElement("issuer"), BsonRepresentation(BsonType.String)]
        public string Issuer { get; set; }

        [BsonElement("audience"), BsonRepresentation(BsonType.String)]
        public string Audience { get; set; }
    }
}
