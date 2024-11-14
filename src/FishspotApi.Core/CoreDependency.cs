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
        }
    }
}
