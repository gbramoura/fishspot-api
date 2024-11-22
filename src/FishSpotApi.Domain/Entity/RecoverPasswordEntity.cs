using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FishSpotApi.Domain.Entity;

public class RecoverPasswordEntity : BaseEntity
{
    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("token")]
    public string Token { get; set; }

    [BsonElement("expiration_date"), BsonRepresentation(BsonType.DateTime)]
    public DateTime ExpirationDate { get; set; }
}