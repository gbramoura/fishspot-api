using FishSpotApi.Data.Context;
using FishSpotApi.Domain.Entity;
using FishSpotApi.Domain.Projection;
using MongoDB.Driver;

namespace FishSpotApi.Core.Repository;

public class SpotRepository(FishSpotApiContext mongo) : BaseRepository<SpotEntity>(mongo, "spot")
{
    public IEnumerable<SpotLocationProjection> GetLocations()
    {
        // TODO: Make a huge filter about the location near to the x and y provided
        var filter = Builders<SpotEntity>.Filter.Empty;
        var projection = Builders<SpotEntity>.Projection
            .Include(entity => entity.Coordinates);

        var locations = _db.Find(filter)
            .Project(projection)
            .ToEnumerable()
            .Select(entity => new SpotLocationProjection
            {
                Id = entity.GetValue("_id").AsString,
                Coordinates = entity.GetValue("coordinates").AsBsonArray.Select(p => p.AsDouble)
            });

        return locations;
    }
}