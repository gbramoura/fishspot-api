using FishSpotApi.Core.Repository;
using FishSpotApi.Domain.Entity;
using FishSpotApi.Domain.Entity.Objects;
using FishSpotApi.Domain.Exception;
using FishSpotApi.Domain.Http.Request;
using FishSpotApi.Domain.Http.Response;
using Microsoft.AspNetCore.Http;

namespace FishSpotApi.Core.Services;

public class SpotService(SpotRepository spotRepository, UserRepository userRepository, FileService fileService)
{
    public SpotCreatedResponse CreateSpot(CreateSpotRequest createSpotRequest, string userId)
    {
        var user = userRepository.Get(userId);
        if (user is null)
        {
            throw new UserNotFoundException("User not found");
        }

        var spotUser = new SpotUser()
        {
            Id = user.Id,
            Name = user.Name
        };
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
        });
        
        var spot = spotRepository.Insert(new SpotEntity
        {
            Images = [],
            User = spotUser,
            Fishes = fishes,
            LocationRisk = risk,
            Title = createSpotRequest.Title,
            Observation = createSpotRequest.Observation,
            Coordinates = createSpotRequest.Coordinates,
            LocationDifficulty = difficulty,
        });

        return new SpotCreatedResponse
        {
            Id = spot.Id,
            Coordinates = spot.Coordinates,
        };
    }

    public SpotResponse GetSpot(string id)
    {
        var spot = spotRepository.Get(id);

        if (spot is null)
        {
            throw new SpotNotFoundException("Spot not found");
        }

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
        });

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
    
    public IEnumerable<SpotLocationResponse> GetUserLocations(string userId, ListRequest listRequest)
    {
        var locations = spotRepository.GetUserLocations(userId, listRequest.PageSize, listRequest.PageNumber);
        var response = locations.Select(location => new SpotLocationResponse
        {
            Id = location.Id,
            Coordinates = location.Coordinates
        });

        return response;
    }
    
    public void DeleteSpot(string id, string userId)
    {
        var spot = spotRepository.Get(id);

        if (spot is null)
        {
            throw new SpotNotFoundException("Spot not found");
        }

        if (userId != spot.User.Id)
        {
            throw new UserNotAuthorizedException("User is not authorized to delete spot");
        }

        foreach (var spotImage in spot.Images)
        {
            fileService.DeleteFile(spotImage);
        }
        
        spotRepository.Delete(id);
    }
}
