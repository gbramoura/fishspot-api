using MongoDB.Bson.Serialization.Attributes;

namespace FishSpotApi.Domain.Entity;

public class TokenEntity : BaseEntity
{
    [BsonElement("actor")]
    public string Actor { get; set; }

    [BsonElement("refresh_token")]
    public string RefreshToken { get; set; }

    [BsonElement("token")]
    public string Token { get; set; }
}