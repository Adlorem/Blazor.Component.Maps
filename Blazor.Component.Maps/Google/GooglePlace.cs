using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blazor.Component.Maps
{
    public class AddressComponent
    {
        [JsonPropertyName("long_name")]
        public string LongName { get; set; }

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonPropertyName("types")]
        public List<string> Types { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }

    public class Viewport
    {
        [JsonPropertyName("south")]
        public double South { get; set; }

        [JsonPropertyName("west")]
        public double West { get; set; }

        [JsonPropertyName("north")]
        public double North { get; set; }

        [JsonPropertyName("east")]
        public double East { get; set; }
    }

    public class Geometry
    {
        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("viewport")]
        public Viewport Viewport { get; set; }
    }

    public class Photo
    {
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("html_attributions")]
        public List<string> HtmlAttributions { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }
    }

    public class GooglePlace
    {
        [JsonPropertyName("address_components")]
        public List<AddressComponent> AddressComponents { get; set; }

        [JsonPropertyName("adr_address")]
        public string AdrAddress { get; set; }

        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("icon_background_color")]
        public string IconBackgroundColor { get; set; }

        [JsonPropertyName("icon_mask_base_uri")]
        public string IconMaskBaseUri { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("photos")]
        public List<Photo> Photos { get; set; }

        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("types")]
        public List<string> Types { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("utc_offset")]
        public int UtcOffset { get; set; }

        [JsonPropertyName("vicinity")]
        public string Vicinity { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("html_attributions")]
        public List<object> HtmlAttributions { get; set; }

        [JsonPropertyName("utc_offset_minutes")]
        public int UtcOffsetMinutes { get; set; }
    }
}
