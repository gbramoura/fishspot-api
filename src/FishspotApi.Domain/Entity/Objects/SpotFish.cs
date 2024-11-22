using MongoDB.Bson.Serialization.Attributes;

namespace FishSpotApi.Domain.Entity;

public class SpotFish
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("weight")]
    public double Weight { get; set; }

    [BsonElement("unit_measure")]
    public string UnitMeasure { get; set; }

    [BsonElement("lures")]
    public IEnumerable<string> Lures { get; set; }
}