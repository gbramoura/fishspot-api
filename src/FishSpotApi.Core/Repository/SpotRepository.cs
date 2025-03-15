using FishSpotApi.Core.Utils;
using FishSpotApi.Data.Context;
using FishSpotApi.Domain.Entity;
using FishSpotApi.Domain.Projection;
using MongoDB.Driver;

namespace FishSpotApi.Core.Repository;

public class SpotRepository(FishSpotApiContext mongo) : BaseRepository<SpotEntity>(mongo, "spot")
{
    public List<SpotLocationProjection> GetLocations()
    {
        // TODO: Make a huge filter about the location near to the x and y provided
        var filter = Builders<SpotEntity>.Filter.Empty;
        var projection = Builders<SpotEntity>.Projection
            .Include(entity => entity.Coordinates);

        var locations = _db.Find(filter)
            .Project(projection)
            .ToList()
            .Select(entity => new SpotLocationProjection
            {
                Id = entity.GetValue("_id").AsObjectId.ToString(),
                Title = entity.GetValue("title").AsObjectId.ToString(),
                Coordinates = entity.GetValue("coordinates").AsBsonArray.Select(p => p.AsDouble).ToList()
            })
            .ToList();

        return locations;
    }
    
    public List<SpotLocationProjection> GetUserLocations(string userId, int pageSize, int pageNumber)
    {
        var filter = Builders<SpotEntity>.Filter.Eq(entity => entity.User.Id, userId);
        var projection = Builders<SpotEntity>.Projection
            .Include(entity => entity.Coordinates);

        var locations = _db.Find(filter)
            .Skip((pageNumber-1) * pageSize)
            .Limit(pageSize)
            .Project(projection)
            .ToEnumerable()
            .Select(entity => new SpotLocationProjection
            {
                Id = entity.GetValue("_id").AsObjectId.ToString(),
                Title = entity.GetValue("title").AsObjectId.ToString(),
                Coordinates = entity.GetValue("coordinates").AsBsonArray.Select(p => p.AsDouble).ToList()
            })
            .ToList();

        return locations;
    }

    public SpotDetailsProjection GetSpotDetailsByUser(string userId)
    {
        var filter = Builders<SpotEntity>.Filter.Eq(entity => entity.User.Id, userId);
        var projection = Builders<SpotEntity>.Projection
            .Include(entity => entity.Fishes);

        var spotsCount = _db.Find(filter).CountDocuments();
        var fishesCount = _db.Find(filter)
            .Project(projection)
            .ToList()
            .SelectMany(entity => MongoJsonUtils.GetListOfBsonDocuments(entity, "fishes"))
            .Select(entity => entity.GetValue("name").AsString)
            .ToList().Distinct().Count();
        
        var luresCount = _db.Find(filter)
            .Project(projection)
            .ToList()
            .SelectMany(entity => MongoJsonUtils.GetListOfBsonDocuments(entity, "fishes"))
            .SelectMany(entity => entity.GetValue("lures").AsBsonArray.Select(p => p.AsString))
            .ToList().Distinct().Count();
        
        return new SpotDetailsProjection()
        {
            Registries = Convert.ToInt32(spotsCount),
            Fishes = fishesCount,
            Lures = luresCount,
        };
    }
}