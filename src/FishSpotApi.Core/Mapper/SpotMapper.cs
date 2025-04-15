using FishSpotApi.Domain.Entity;
using FishSpotApi.Domain.Entity.Objects;
using FishSpotApi.Domain.Http.Request;
using FishSpotApi.Domain.Http.Response;

namespace FishSpotApi.Core.Mapper;

public static class SpotMapper
{
    public static SpotEntity CreateSpotRequestToEntity(CreateSpotRequest createSpotRequest, SpotUser user)
    {
        var risk = new SpotLocationRisk()
        {
            Rate = createSpotRequest.LocationRisk.Rate,
            Observation = createSpotRequest.LocationRisk.Observation
        };
        var difficulty = new SpotLocationDifficulty()
        {
            Rate = createSpotRequest.LocationDifficulty.Rate,
            Observation = createSpotRequest.LocationDifficulty.Observation
        };
        var fishes = createSpotRequest.Fishes.Select(fish => new SpotFish()
        {
            Name = fish.Name,
            Weight = fish.Weight,
            UnitMeasure = fish.UnitMeasure,
            Lures = fish.Lures,
        }).ToList();
        
        return new SpotEntity
        {
            Images = [],
            User = user,
            Fishes = fishes,
            LocationRisk = risk,
            Title = createSpotRequest.Title,
            Observation = createSpotRequest.Observation,
            Coordinates = createSpotRequest.Coordinates,
            LocationDifficulty = difficulty,
            Date = createSpotRequest.Date,
        };
    }

    public static SpotEntity UpdateSpotRequestToEntity(UpdateSpotRequest updateSpotRequest, SpotUser user, List<string> images)
    {
        var risk = new SpotLocationRisk()
        {
            Rate = updateSpotRequest.LocationRisk.Rate,
            Observation = updateSpotRequest.LocationRisk.Observation
        };
        var difficulty = new SpotLocationDifficulty()
        {
            Rate = updateSpotRequest.LocationDifficulty.Rate,
            Observation = updateSpotRequest.LocationDifficulty.Observation
        };
        var fishes = updateSpotRequest.Fishes.Select(fish => new SpotFish()
        {
            Name = fish.Name,
            Weight = fish.Weight,
            UnitMeasure = fish.UnitMeasure,
            Lures = fish.Lures,
        }).ToList();

        return new SpotEntity
        {
            User = user,
            Images = images,
            Fishes = fishes,
            LocationRisk = risk,
            LocationDifficulty = difficulty,
            Title = updateSpotRequest.Title,
            Observation = updateSpotRequest.Observation,
            Coordinates = updateSpotRequest.Coordinates,
            Date = updateSpotRequest.Date,
        };
    }
    
    public static SpotResponse SpotEntityToResponse(SpotEntity spot)
    {
        var difficulty = new SpotRateResponse
        {
            Rate = spot.LocationRisk.Rate.ToString(),
            Observation = spot.LocationRisk.Observation
        };
        var risk = new SpotRateResponse
        {
            Rate = spot.LocationDifficulty.Rate.ToString(),
            Observation = spot.LocationDifficulty.Observation
        };
        var fishes = spot.Fishes.Select(fish => new SpotFishResponse
        {
            Name = fish.Name,
            Weight = fish.Weight,
            UnitMeasure = fish.UnitMeasure,
            Lures = fish.Lures,
        }).ToList();

        return new SpotResponse()
        {
            Title = spot.Title,
            Observation = spot.Observation,
            Coordinates = spot.Coordinates,
            Images = spot.Images,
            LocationRisk = difficulty,
            LocationDifficulty = risk,
            Fishes = fishes,
            User = new SpotUserResponse()
            {
                Name = spot.User.Name,
            }
        };
    }
}