using System.Security.Claims;
using System.Security.Principal;

namespace FishSpotApi.Core.Extension;

public static class IdentitySExtension
{
    public static string GetUserId(this IIdentity identity)
    {
        return ((ClaimsIdentity)identity).FindFirst("id")?.Value ?? string.Empty;
    }
}