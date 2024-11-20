using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FishspotApi.Domain.Entity
{
    public class SpotFish
    {
        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string Name { get; set; }

        [BsonElement("weight"), BsonRepresentation(BsonType.Double)]
        public double Weight { get; set; }

        [BsonElement("unit_measure"), BsonRepresentation(BsonType.String)]
        public string UnitMeasure { get; set; }

        [BsonElement("lures"), BsonRepresentation(BsonType.Array)]
        public IEnumerable<string> Lures { get; set; }
    }
}