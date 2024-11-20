using FishspotApi.Domain.Entity.Objects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FishspotApi.Domain.Entity
{
    public class SpotEntity : BaseEntity
    {
        [BsonElement("title"), BsonRepresentation(BsonType.String)]
        public string Title { get; set; }

        [BsonElement("observation"), BsonRepresentation(BsonType.String)]
        public string Observation { get; set; }

        [BsonElement("coordinates"), BsonRepresentation(BsonType.Array)]
        public IEnumerable<double> Coordinates { get; set; }

        [BsonElement("location_dificulty"), BsonRepresentation(BsonType.Document)]
        public SpotLocationDificulty LocationDificulty { get; set; }

        [BsonElement("location_risk"), BsonRepresentation(BsonType.Document)]
        public SpotLocationRisk LocationRisk { get; set; }

        [BsonElement("images"), BsonRepresentation(BsonType.Array)]
        public IEnumerable<string> Images { get; set; }

        [BsonElement("fishs"), BsonRepresentation(BsonType.Array)]
        public IEnumerable<SpotFish> Fishs { get; set; }

        [BsonElement("user"), BsonRepresentation(BsonType.Document)]
        public SpotUser User { get; set; }
    }
}