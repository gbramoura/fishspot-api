using MongoDB.Bson.Serialization.Attributes;

namespace FishSpotApi.Domain.Entity.Objects;

public class UserAddress
{
    [BsonElement("street")]
    public string Street { get; set; }
    
    [BsonElement("number")]
    public int Number { get; set; }
    
    [BsonElement("neighborhood")]
    public string Neighborhood { get; set; }
    
    [BsonElement("zip_code")]
    public string ZipCode { get; set; }
}