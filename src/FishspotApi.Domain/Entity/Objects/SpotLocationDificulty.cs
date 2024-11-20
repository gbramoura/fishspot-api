using FishspotApi.Domain.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FishspotApi.Domain.Entity
{
    public class SpotLocationDificulty
    {
        [BsonElement("rate"), BsonRepresentation(BsonType.Int32)]
        public SpotLocationDificultyRate Rate { get; set; }

        [BsonElement("rate"), BsonRepresentation(BsonType.String)]
        public string Observation { get; set; }
    }
}