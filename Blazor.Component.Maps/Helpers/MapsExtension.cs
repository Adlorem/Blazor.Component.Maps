﻿using Microsoft.Extensions.DependencyInjection;

using System;

namespace Blazor.Component.Maps
{
    /// <summary>
    /// Extension methods to register required Google, Bing, etc map services into IServiceCollection
    /// </summary>
    public static class MapsExtension
    {
        /// <summary>
        /// Registers required Map services into IServiceCollection
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        public static IServiceCollection AddMapExtensions(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IGoogleMapService, GoogleMapService>();

            return services;
        }
    }
}