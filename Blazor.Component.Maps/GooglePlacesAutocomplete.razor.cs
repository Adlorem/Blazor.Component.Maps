using Microsoft.AspNetCore.Components;

namespace Blazor.Component.Maps
{
    public partial class GooglePlacesAutocomplete: IAsyncDisposable
    {

        [Parameter]
        public EventCallback<GooglePlace> OnPlaceChanged { get; set; }
        [Parameter]
        public string ApiKey { get; set; }
        [Parameter]
        public string Class { get; set; }

        private string _inputElementId = $"input_{Guid.NewGuid().ToString("n")}";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await _googlePlacesService.InitAutocompleteAsync(apiKey: ApiKey,
                textBoxElementId: _inputElementId,
                textBoxInitializedCallback: null,
                placeChangedCallback: PlaceChanged);
            }
        }

        private Task PlaceChanged(GooglePlace place, string textBoxElementId)
        {
            if (place != null)
            {
                OnPlaceChanged.InvokeAsync(place);
            }
            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_googlePlacesService is not null)
            {
                await _googlePlacesService.DisposeAsync();
            }
        }
    }
}
