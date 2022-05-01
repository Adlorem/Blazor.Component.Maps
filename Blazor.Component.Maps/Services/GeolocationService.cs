using Blazor.Component.Maps.Geolocation;
using Microsoft.JSInterop;

namespace Blazor.Component.Maps
{
    /// <summary>
    /// Implementation of <see cref="IGeolocationService"/>
    /// </summary>
    public sealed class GeolocationService : IGeolocationService
    {
        private const int DefaultTimeOut = 5000;
        private const int DefaultCacheTime = 0;

        private readonly IJSRuntime _jsRuntime;
        private IJSObjectReference _geoJs;
        private List<DotNetObjectReference<GeolocationEventWatcherInfo>> _dotNetObjectReferences;

        public GeolocationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _dotNetObjectReferences = new List<DotNetObjectReference<GeolocationEventWatcherInfo>>();
        }

        public async Task GetCurrentPositionAsync(Func<GeolocationResult, Task> locationResultCallback, bool highAccuracy, TimeSpan? timeout, TimeSpan? cacheTime)
        {
            await CheckJsObjectAsync();

            var info = new GeolocationEventCurrentPositionInfo(locationResultCallback);
            var dotnetRef = DotNetObjectReference.Create(info);
            info.DotNetObjectReference = dotnetRef; //Store ref to self dispose.

            await _geoJs.InvokeVoidAsync("getCurrentPosition", dotnetRef,
                highAccuracy,
                timeout?.TotalMilliseconds ?? DefaultTimeOut,
                cacheTime?.TotalMilliseconds ?? DefaultCacheTime);
        }

        public async Task<int> AddGeolocationWatcherAsync(Func<GeolocationResult, Task> locationEventsCallback, bool highAccuracy = false, TimeSpan? timeout = null, TimeSpan? cacheTime = null)
        {
            await CheckJsObjectAsync();

            var info = new GeolocationEventWatcherInfo(locationEventsCallback);
            var dotnetRef = DotNetObjectReference.Create(info);

            var id = await _geoJs.InvokeAsync<int>("addGeolocationWatcher", dotnetRef,
                highAccuracy,
                timeout?.TotalMilliseconds ?? DefaultTimeOut,
                cacheTime?.TotalMilliseconds ?? DefaultCacheTime);

            dotnetRef.Value.HandlerId = id;
            _dotNetObjectReferences.Add(dotnetRef);
            return id;
        }

        public async Task RemoveGeolocationWatcherAsync(int handlerId)
        {
            await CheckJsObjectAsync();

            await _geoJs.InvokeVoidAsync("removeGeolocationWatcher", handlerId);

            var dotNetRefs = _dotNetObjectReferences.Where(x => x.Value.HandlerId == handlerId);
            _dotNetObjectReferences = _dotNetObjectReferences.Except(dotNetRefs).ToList();

            foreach (var item in dotNetRefs)
            {
                item.Dispose();
            }
        }

        private async Task CheckJsObjectAsync()
        {
            if (_geoJs is null)
            {
#if DEBUG
                _geoJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.Component.Maps/geo.js");
#else
				_geoJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.Component.Maps/geo.min.js");
#endif
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_geoJs is not null)
            {
                await _geoJs.InvokeVoidAsync("dispose",
                    _dotNetObjectReferences.Select(s => s.Value.HandlerId).Distinct().ToArray());

                await _geoJs.DisposeAsync();
            }

            foreach (var item in _dotNetObjectReferences)
            {
                item.Dispose();
            }
        }
    }
}
