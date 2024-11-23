using FishSpotApi.Domain.Enum;
using MongoDB.Bson.Serialization.Attributes;

namespace FishSpotApi.Domain.Entity;

public class SpotLocationRisk
{
    [BsonElement("rate")]
    public SpotLocationRiskRate Rate { get; set; }

    [BsonElement("observation")]
    public string Observation { get; set; }
}