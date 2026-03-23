using FishSpotApi.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FishSpotApi.Data;

public static class DataDependency
{
    /// <summary>
    /// Method to include the context of data lib into depency injection
    /// </summary>
    /// <param name="services">Extesion of IServiceCollection</param>
    public static void AddDatabaseDependency(this IServiceCollection services)
    {
        services.AddSingleton<FishSpotApiContext>();
    }

    public static void StartDatabaseConnection(this IApplicationBuilder app)
    {
        app.ApplicationServices.GetRequiredService<FishSpotApiContext>();
    }
}