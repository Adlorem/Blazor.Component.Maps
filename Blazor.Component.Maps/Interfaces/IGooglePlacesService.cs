using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Component.Maps
{
    public interface IGooglePlacesService : IAsyncDisposable
    {
        public string TextBoxElementId { get;}
        Task InitAutocompleteAsync(string apiKey,
            string textBoxElementId,
            Func<string, Task> textBoxInitializedCallback = null,
            Func<GooglePlace, string, Task> placeChangedCallback = null
            );
    }
}
