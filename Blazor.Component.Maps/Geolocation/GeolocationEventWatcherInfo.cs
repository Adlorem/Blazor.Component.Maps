namespace Blazor.Component.Maps.Geolocation
{
    /// <summary>
    /// Implementation of <see cref="GeolocationEventInfo"/> for AddGeolocationWatcher method.
    /// </summary>
    internal sealed class GeolocationEventWatcherInfo : GeolocationEventInfo
    {
        public int HandlerId { get; set; }

        public GeolocationEventWatcherInfo(Func<GeolocationResult, Task> locationResultCallback)
            : base(locationResultCallback)
        {
        }
    }
}