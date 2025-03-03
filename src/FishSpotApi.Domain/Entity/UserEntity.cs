using FishSpotApi.Domain.Entity.Objects;
using MongoDB.Bson.Serialization.Attributes;

namespace FishSpotApi.Domain.Entity;

public class UserEntity : BaseEntity
{
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("username")]
    public string Username { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("password")]
    public string Password { get; set; }
    
    [BsonElement("description")]
    public string Description { get; set; }
    
    [BsonElement("image")]
    public string Image { get; set; }
    
    [BsonElement("address")]
    public UserAddress Address { get; set; }
    
    [BsonElement("Unique_identifier_token")]
    public string UniqueIdentifierToken { get; set; }
}