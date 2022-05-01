using Blazor.Component.Maps;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Utilities;

namespace MudBlazor
{
    public partial class MudGooglePlacesAutocomplete<T> : MudDebouncedInput<T>, IAsyncDisposable
    {
        [Parameter]
        public EventCallback<GooglePlace> OnPlaceChanged { get; set; }
        [Parameter]
        public string ApiKey { get; set; }
        /// <summary>
        /// Type of the input element. It should be a valid HTML5 input type.
        /// </summary>
        [Parameter]
        [Category(CategoryTypes.FormComponent.Behavior)]
        public InputType InputType { get; set; } = InputType.Text;

        /// <summary>
        /// Show clear button.
        /// </summary>
        [Parameter]
        [Category(CategoryTypes.FormComponent.Behavior)]
        public bool Clearable { get; set; } = false;

        /// <summary>
        /// Button click event for clear button. Called after text and value has been cleared.
        /// </summary>
        [Parameter] public EventCallback<MouseEventArgs> OnClearButtonClick { get; set; }

        internal bool Standalone { get; set; } = true;

        public MudInput<string> InputReference { get; private set; }


        private string _inputElementId = $"input_{Guid.NewGuid().ToString("n")}";

        private string _required = string.Empty;
        private MudMask _maskReference;

        private string GetCounterText() => Counter == null ? string.Empty : (Counter == 0 ? (string.IsNullOrEmpty(Text) ? "0" : $"{Text.Length}") : ((string.IsNullOrEmpty(Text) ? "0" : $"{Text.Length}") + $" / {Counter}"));

        protected string Classname =>
            new CssBuilder("mud-input-input-control")
            .AddClass(Class)
            .Build();

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

        private IMask _mask = null;

        /// <summary>
        /// Provide a masking object. Built-in masks are PatternMask, MultiMask, RegexMask and BlockMask
        /// Note: when Mask is set, TextField will ignore some properties such as Lines, Pattern or HideSpinButtons, OnKeyDown and OnKeyUp, etc.
        /// </summary>
        [Parameter]
        [Category(CategoryTypes.General.Data)]
        public IMask Mask
        {
            get => _maskReference?.Mask ?? _mask; // this might look strange, but it is absolutely necessary due to how MudMask works.
            set
            {
                _mask = value;
            }
        }

        protected override Task SetTextAsync(string text, bool updateValue = true)
        {
            if (_mask != null)
            {
                _mask.SetText(text);
                text = _mask.Text;
            }
            return base.SetTextAsync(text, updateValue);
        }

        private async Task OnMaskedValueChanged(string s)
        {
            await SetTextAsync(s);
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
