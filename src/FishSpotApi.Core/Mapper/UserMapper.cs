using FishSpotApi.Domain.Entity;
using FishSpotApi.Domain.Entity.Objects;
using FishSpotApi.Domain.Http.Request;
using FishSpotApi.Domain.Http.Response;
using FishSpotApi.Domain.Projection;

namespace FishSpotApi.Core.Mapper;

public class UserMapper
{
    public static UserResponse UserEntityToUserResponse(UserEntity userEntity)
    {
        var address = new UserAddressResponse()
        {
            Neighborhood = userEntity.Address?.Neighborhood ?? string.Empty,
            Street = userEntity.Address?.Street ?? string.Empty,
            Number = userEntity.Address?.Number ?? 0,
            ZipCode = userEntity.Address?.ZipCode ?? string.Empty,
        };
        
        return new UserResponse()
        {
            Name = userEntity.Name,
            Email = userEntity.Email,
            Description = userEntity.Description,
            Username = userEntity.Username,
            Image = userEntity.Image,
            Address = address
        };
    }

    public static UserResponse SpotDetailsToUserResponse(SpotDetailsProjection spotDetails, UserResponse userResponse)
    {
        userResponse.SpotDetails = new UserSpotDetailsResponse()
        {
            Fishes = spotDetails.Fishes,
            Lures = spotDetails.Lures,
            Registries = spotDetails.Registries,
        };
        
        return userResponse;
    }

    public static UserEntity UserUpdateRequestToUserEntity(UserEntity user, UserUpdateRequest update)
    {
        var address = new UserAddress()
        {
            Number = update.Address.Number,
            Neighborhood = update.Address.Neighborhood,
            Street = update.Address.Street,
            ZipCode = update.Address.ZipCode,
        };

        return new UserEntity()
        {
            Name = update.Name,
            Description = update.Description,
            Username = update.Username ?? user.Username,
            Image = user.Image,
            Email = user.Email,
            Address = update.Address != null ? user.Address : address,
        };
    }
}