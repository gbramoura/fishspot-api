﻿using FishspotApi.Data.Context;
using Microsoft.Extensions.DependencyInjection;

namespace FishspotApi.Data
{
    public static class DataDependency
    {
        /// <summary>
        /// Method to include the context of data lib into depency injection
        /// </summary>
        /// <param name="services">Extesion of IServiceCollection</param>
        public static void AddDataDependency(this IServiceCollection services)
        {
            services.AddSingleton<FishspotApiContext>();
        }
    }
}
