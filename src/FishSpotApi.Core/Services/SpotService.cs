using FishSpotApi.Core.Mapper;
using FishSpotApi.Core.Repository;
using FishSpotApi.Domain.Entity;
using FishSpotApi.Domain.Entity.Objects;
using FishSpotApi.Domain.Exception;
using FishSpotApi.Domain.Http.Request;
using FishSpotApi.Domain.Http.Response;
using FishSpotApi.Domain.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace FishSpotApi.Core.Services;

public class SpotService(SpotRepository spotRepository, UserRepository userRepository, FileService fileService, IStringLocalizerFactory factory)
{
    private readonly IStringLocalizer _localizer = factory.Create(typeof(FishSpotResource));
    public SpotCreatedResponse CreateSpot(CreateSpotRequest createSpotRequest, string userId)
    {
        var user = userRepository.Get(userId);
        if (user is null)
        {
            throw new UserNotFoundException(_localizer["spot_not_found"]);
        }

        var spotUser = new SpotUser()
        {
            Id = user.Id,
            Name = user.Name
        };
        var spotEntity = SpotMapper.CreateSpotRequestToEntity(createSpotRequest, spotUser);
        var spot = spotRepository.Insert(spotEntity);

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
            throw new SpotNotFoundException(_localizer["spot_not_found"]);
        }

        return SpotMapper.SpotEntityToResponse(spot);
    }

    public IEnumerable<SpotLocationResponse> GetNearLocations()
    {
        // TODO: include the near location
        var locations = spotRepository.GetLocations();
        var response = locations.Select(location => new SpotLocationResponse
        {
            Id = location.Id,
            Title = location.Title,
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
            Title = location.Title,
            Coordinates = location.Coordinates
        });

        return response;
    }

    public void UpdateSpot(string spotId, UpdateSpotRequest updateSpotRequest, string userId)
    {
        var spot = spotRepository.Get(spotId);
        if (spot is null)
        {
            throw new SpotNotFoundException(_localizer["spot_not_found"]);
        }

        if (userId != spot.User.Id)
        {
            throw new UserNotAuthorizedException(_localizer["user_not_authorized_to_update_spot"]);
        }
        
        var spotEntity = SpotMapper.UpdateSpotRequestToEntity(updateSpotRequest, spot.User, spot.Images);
        spotRepository.Update(spotEntity);
    }
    
    public void DeleteSpot(string id, string userId)
    {
        var spot = spotRepository.Get(id);
        if (spot is null)
        {
            throw new SpotNotFoundException(_localizer["spot_not_found"]);
        }

        if (userId != spot.User.Id)
        {
            throw new UserNotAuthorizedException(_localizer["user_not_authorized_to_delete_spot"]);
        }
    
        spot.Images.ForEach(fileService.DeleteFile);
        spotRepository.Delete(id);
    }
}
