using FishSpotApi.Domain.Enum;
using MongoDB.Bson.Serialization.Attributes;

namespace FishSpotApi.Domain.Entity;

public class SpotFish
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("weight")]
    public double Weight { get; set; }

    [BsonElement("unit_measure")]
    public UnitMeasureType UnitMeasure { get; set; }

    [BsonElement("lures")]
    public List<string> Lures { get; set; }
}