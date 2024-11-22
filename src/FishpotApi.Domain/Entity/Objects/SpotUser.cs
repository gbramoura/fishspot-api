using MongoDB.Bson.Serialization.Attributes;

namespace FishSpotApi.Domain.Entity.Objects;

public class SpotUser
{
    [BsonElement("id")]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }
}