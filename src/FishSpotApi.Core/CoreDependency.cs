using FishSpotApi.Core.Repository;
using FishSpotApi.Core.Services;
using FishSpotApi.Data;
using Microsoft.Extensions.DependencyInjection;

namespace FishSpotApi.Core;

public static class CoreDependency
{
    /// <summary>
    /// Method to include the services of core lib into depency injection
    /// </summary>
    /// <param name="services">Extesion of IServiceCollection</param>
    public static void AddCoreDependency(this IServiceCollection services)
    {
        services.AddDataDependency();
        services.AddScoped<TokenRepository>();
        services.AddScoped<UserRepository>();
        services.AddScoped<RecoverPasswordRepository>();
        services.AddScoped<SpotRepository>();
        services.AddScoped<TokenService>();
        services.AddScoped<UserService>();
        services.AddScoped<MailService>();
        services.AddScoped<SpotService>();
        services.AddScoped<FileService>();
    }
}