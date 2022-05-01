
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Component.Maps
{
    public static class MapServiceExtensions
    {
        public static IServiceCollection AddMapsServices(this IServiceCollection services)
        {
            services.AddTransient<IGeolocationService, GeolocationService>();
            services.AddTransient<IGoogleMapService, GoogleMapService>();
            services.AddTransient<IGooglePlacesService, GooglePlacesService>();
            return services;
        }
    }


}
