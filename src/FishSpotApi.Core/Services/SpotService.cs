using FishSpotApi.Core.Repository;
using FishSpotApi.Domain.Exception;
using FishSpotApi.Domain.Http.Response;

namespace FishSpotApi.Core.Services;

public class SpotService(SpotRepository spotRepository)
{
    public IEnumerable<SpotLocationResponse> GetNearLocations()
    {
        // TODO: include the near location
        var locations = spotRepository.GetLocations();
        var response = locations.Select(location => new SpotLocationResponse
        {
            Id = location.Id,
            Coordinates = location.Coordinates
        });

        return response;
    }

    public SpotResponse GetSpot(string id)
    {
        var spot = spotRepository.Get(id);

        if (spot is null)
        {
            throw new SpotNotFoundException("Spot not found");
        }

        var response = new SpotResponse()
        {
            Title = spot.Title,
            Observation = spot.Observation,
            Coordinates = spot.Coordinates,
            Images = spot.Images,
            User = new SpotUserResponse()
            {
                Name = spot.User.Name,
            },
            LocationRisk = new SpotRateResponse
            {
                Rate = spot.LocationRisk.Rate.ToString(),
                Observation = spot.LocationRisk.Observation
            },
            LocationDifficulty = new SpotRateResponse
            {
                Rate = spot.LocationDifficulty.Rate.ToString(),
                Observation = spot.LocationDifficulty.Observation
            },
            Fishs = spot.Fishs.Select(fish => new SpotFishResponse
            {
                Name = fish.Name,
                Weight = fish.Weight,
                UnitMeasure = fish.UnitMeasure,
                Lures = fish.Lures,
            })
        };

        return response;
    }

    public void DeleteSpot(string id)
    {
        var spot = spotRepository.Get(id);

        if (spot is null)
        {
            throw new SpotNotFoundException("Spot not found");
        }

        spotRepository.Delete(id);
    }
}
