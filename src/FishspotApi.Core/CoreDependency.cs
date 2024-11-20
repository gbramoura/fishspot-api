using FishspotApi.Core.Repository;
using FishspotApi.Core.Services;
using FishspotApi.Data;
using Microsoft.Extensions.DependencyInjection;

namespace FishspotApi.Core
{
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
        }
    }
}