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
    public void CreateSpot(CreateSpotRequest createSpotRequest, string userId)
    {
        var user = userRepository.Get(userId);
        if (user is null)
        {
            throw new UserNotFoundException("User not found");
        }
        
        var savedImages = new List<string>();
        if (createSpotRequest.Images.Any())
        {
            savedImages.AddRange(SaveSpotImages(createSpotRequest.Images));
        }

        spotRepository.Insert(new SpotEntity
        {
            Images = savedImages,
            Title = createSpotRequest.Title,
            Observation = createSpotRequest.Observation,
            Coordinates = createSpotRequest.Coordinates,
            User = new SpotUser()
            {
                Id = user.Id,
                Name = user.Name
            },
            Fishes = createSpotRequest.Fishes.Select(fish => new SpotFish()
            {
                Name = fish.Name,
                Weight = fish.Weight,
                UnitMeasure = fish.UnitMeasure,
                Lures = fish.Lures,
            }),
            LocationDifficulty = new SpotLocationDifficulty()
            {
                Rate = createSpotRequest.LocationDifficulty.Rate,
                Observation = createSpotRequest.LocationDifficulty.Observation
            },
            LocationRisk = new SpotLocationRisk()
            {
                Rate = createSpotRequest.LocationRisk.Rate,
                Observation = createSpotRequest.LocationRisk.Observation
            },
        });
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
            Fishs = spot.Fishes.Select(fish => new SpotFishResponse
            {
                Name = fish.Name,
                Weight = fish.Weight,
                UnitMeasure = fish.UnitMeasure,
                Lures = fish.Lures,
            })
        };

        return response;
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

        spotRepository.Delete(id);
    }

    private IEnumerable<string> SaveSpotImages(IEnumerable<IFormFile> files)
    {
        var allowedFileExtensions = new String[] { ".jpg", ".jpeg", ".png" };
        var savedFiles = new List<string>();
        try
        {
            foreach (var file in files)
            {
                savedFiles.Add(fileService.SaveFile(file, allowedFileExtensions));
            }

            return savedFiles;
        }
        catch (Exception e)
        {
            foreach (var file in savedFiles)
            {
                fileService.DeleteFile(file);
            }
            throw new InvalidImageException("Unable to save files", e); 
        }
    }
    
}
