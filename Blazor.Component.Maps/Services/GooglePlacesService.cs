using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Component.Maps
{
    public class GooglePlacesService : IGooglePlacesService
    {
        private readonly IJSRuntime _jsRuntime;
        private IJSObjectReference _autocompleteJs;
        private DotNetObjectReference<GooglePlaceEventInfo> _dotNetObjectReference;

        public GooglePlacesService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public string TextBoxElementId { get; private set; }
        public string SearchBoxElementId { get; private set; }

        public async ValueTask DisposeAsync()
        {
            if (_autocompleteJs != null)
            {
                await _autocompleteJs.InvokeVoidAsync("disposeAutocomplete", TextBoxElementId);
                await _autocompleteJs.DisposeAsync();
            }
            _dotNetObjectReference?.Dispose();
        }

        public async Task InitAutocompleteAsync(string apiKey, 
            string textBoxElementId, 
            Func<string, Task> autocompleteInitializedCallback = null, 
            Func<GooglePlace, string, Task> placeChangedCallback = null)
        {
            // Prevent double initialization of element.
            if (TextBoxElementId == textBoxElementId)
            {
                return;
            }

            TextBoxElementId = textBoxElementId;

            await CheckJsObjectAsync();

            var searchBoxId = $"box_{Guid.NewGuid().ToString("n")}";
            SearchBoxElementId = searchBoxId;

            var info = new GooglePlaceEventInfo(textBoxElementId: textBoxElementId,
                autocompleteInitializedCallback: autocompleteInitializedCallback,
                placeChangedCallback: placeChangedCallback);

            _dotNetObjectReference = DotNetObjectReference.Create(info);

            await _autocompleteJs.InvokeVoidAsync("initAutocomplete", apiKey, searchBoxId, textBoxElementId, _dotNetObjectReference);
        }

        private async Task CheckJsObjectAsync()
        {
            if (_autocompleteJs is null)
            {
#if DEBUG
                _autocompleteJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.Component.Maps/googleMaps.js");
#else
				_autocompleteJs = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Blazor.Component.Maps/googleMaps.min.js");
#endif
            }
        }
    }
}
