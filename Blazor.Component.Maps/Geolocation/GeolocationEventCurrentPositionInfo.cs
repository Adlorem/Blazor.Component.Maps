﻿using Microsoft.JSInterop;

namespace Blazor.Component.Maps.Geolocation
{
    /// <summary>
    /// Implementation of <see cref="GeolocationEventInfo"/> for GetCurrentPosition method with self dispose <see cref="DotNetObjectReference"/>
    /// </summary>
    internal sealed class GeolocationEventCurrentPositionInfo : GeolocationEventInfo
    {
        internal DotNetObjectReference<GeolocationEventCurrentPositionInfo> DotNetObjectReference { get; set; }

        public GeolocationEventCurrentPositionInfo(Func<GeolocationResult, Task> locationResultCallback)
            : base(locationResultCallback)
        {
        }

        [JSInvokable("GeolocationEvent")]
        public override async Task GeolocationEvent(GeolocationResult args)
        {
            await base.GeolocationEvent(args);

            if (DotNetObjectReference is not null) //Callback supplied only with 
            {
                DotNetObjectReference.Dispose();
            }
        }
    }
}