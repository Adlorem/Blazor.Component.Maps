using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Component.Maps
{
    internal class GooglePlaceEventInfo
    {
        private string _textBoxElementId;
        private Func<string, Task> _autocompleteInitializedCallback;
        private Func<GooglePlace, string, Task> _placeChangedCallback;

        public GooglePlaceEventInfo(string textBoxElementId, 
            Func<string, Task> autocompleteInitializedCallback, 
            Func<GooglePlace, string, Task> placeChangedCallback)
        {
            _textBoxElementId = textBoxElementId;
            _autocompleteInitializedCallback = autocompleteInitializedCallback;
            _placeChangedCallback = placeChangedCallback;
        }

        //Map evetns
        [JSInvokable("PlacesInitialized")]
        public async Task PlacesInitialized(string textBoxElementId)
        {
            if (_textBoxElementId != textBoxElementId)
            {
                throw new InvalidProgramException($"{nameof(PlacesInitialized)} method was called with invalid id container Div Id: {textBoxElementId}, expected Id is {_textBoxElementId}.");
            }

            if (_autocompleteInitializedCallback is not null)
            {
                await _autocompleteInitializedCallback.Invoke(textBoxElementId);
            }
        }

        [JSInvokable("PlacesChanged")]
        public async Task PlacesChanged(GooglePlace places, string textBoxElementId)
        {
            if (_textBoxElementId != textBoxElementId)
            {
                throw new InvalidProgramException($"{nameof(PlacesInitialized)} method was called with invalid id container Div Id: {textBoxElementId}, expected Id is {_textBoxElementId}.");
            }

            if (_placeChangedCallback is not null)
            {
                await _placeChangedCallback.Invoke(places, textBoxElementId);
            }
        }
    }


}
