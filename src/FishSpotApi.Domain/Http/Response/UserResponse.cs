namespace FishSpotApi.Domain.Http.Response;

public class UserResponse
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public UserSpotDetailsResponse SpotDetails { get; set; }
    public UserAddressResponse Address { get; set; }
}

public class UserAddressResponse
{
    public string Street { get; set; }
    public int Number { get; set; }
    public string Neighborhood { get; set; }
    public string ZipCode { get; set; }
}

public class UserSpotDetailsResponse
{
    public int Registries { get; set; }
    public int Fishes { get; set; }
    public int Lures { get; set; }
}