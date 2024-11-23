﻿using FishSpotApi.Domain.Entity.Objects;
using MongoDB.Bson.Serialization.Attributes;

namespace FishSpotApi.Domain.Entity;

public class SpotEntity : BaseEntity
{
    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("observation")]
    public string Observation { get; set; }

    [BsonElement("coordinates")]
    public IEnumerable<double> Coordinates { get; set; }

    [BsonElement("location_difficulty")]
    public SpotLocationDifficulty LocationDifficulty { get; set; }

    [BsonElement("location_risk")]
    public SpotLocationRisk LocationRisk { get; set; }

    [BsonElement("images")]
    public IEnumerable<string> Images { get; set; }

    [BsonElement("fishes")]
    public required IEnumerable<SpotFish> Fishes { get; set; }

    [BsonElement("user")]
    public SpotUser User { get; set; }
}