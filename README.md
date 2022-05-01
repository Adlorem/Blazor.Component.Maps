
# Blazor Google maps integration with Googla places autocomplete with extra support for MudBlazor.

The aim of this project is to easily integrate google maps with your Blazor application. 
This means separate support for google maps and google places autocompleted that can be bind together. 
Original google maps code was borrowed from this great repository:

https://github.com/majorimi/blazor-components

Please note that original code was modified and this repository is no longer compatible with the original repository.

## Usage/Examples

1. Add library to your project.
2. Modify your Program.cs and add these lines:

```c#
using Blazor.Component.Maps;

builder.Services.AddMapsServices();


```

You can also add only services you are going to use.

3. You are ready to go and use google maps and google autocomplete in your project.

Basic google maps usage 

```c#
<GoogleMap @ref="_googleMap"
            ApiKey="@_yourGoogleApiKey" />


```

Basic google maps autocomplete usage

```c#
<GooglePlacesAutocomplete ApiKey="@_yourGoogleApiKey" />


```
### MudBlazor support

This library was written to support MudBlazor components. In case you intend to use MudBlazor please import additonal library MudGooglePlacesAutocomplete.
This will allow you to use MudGooglePlacesAutocomplete component that fully supports MudBlazor. Please check MudBlazorMapsExample for details.

## Documentation

### Google Maps Component

### Properties
- **`InnerElementReference`: `ElementReference { get; }`** <br />
Exposes a Blazor `ElementReference` of the wrapped around HTML element. It can be used e.g. for JS interop, etc.
- **`MapId`: `string { get; }` (default: false)** <br />
Map HTML container Id. It can be used when multiple Maps added to one page.
- **`UsePlacesLibrary`: `bool {set;}`** <br />
Set to true if you intend to use both map and autocomplete component on the same page.
- **`Width`: `int { get; set; }` (default: 400)** <br />
Maps image Width in px.
- **`Height`: `int { get; set; }` (default: 300)** <br />
Maps image Height in px.
- **`BackgroundColor`: `string? { get; set; }` (default: NULL)** <br />
Color used for the background of the Map div. This color will be visible when tiles have not yet loaded as the user pans.
This option can only be set when the map is initialized.
- **`ControlSize`: `int { get; set; }` (default: 0)** <br />
Size in pixels of the controls appearing on the map. This value must be supplied directly when creating the Map.
- **`CustomControls`: `IEnumerable<GoogleMapCustomControl>? { get; set; }` (default: NULL)** <br />
Custom controls to add to the Map that will execute callbacks for events.
This option can only be set when the map is initialized. Use `OnInitialized` method to set it up.
- **`Markers`: `ObservableRangeCollection<GoogleMapMarker>? { get; set; }` (default: NULL)** <br />
MarkerOptions object used to define the properties that can be set on a Marker.
ObservableCollection can be initialized only once! Add or remove items to the collection the change marker properties (Marker properties value changes not detected).
- **`Zoom`: `byte { get; set; }` (default: 12)** <br />
Defines the zoom level of the map, which determines the magnification level of the map.
- **`ZoomControl`: `bool { get; set; }` (default: true)** <br />
The enabled/disabled state of the Zoom control.
- **`ZoomControlOptionsPosition`: `GoogleMapControlPositions { get; set; }` (default: GoogleMapControlPositions.BOTTOM_RIGHT)** <br />
The display options for the Zoom control.
- **`MaxZoom`: `byte? { get; set; }` (default: NULL)** <br />
The maximum zoom level which will be displayed on the map. If omitted, or set to null,
the maximum zoom from the current map type is used instead.
- **`MinZoom`: `byte? { get; set; }` (default: NULL)** <br />
The minimum zoom level which will be displayed on the map. If omitted, or set to null,
the minimum zoom from the current map type is used instead.
- **`Center`: `GeolocationData? { get; set; }` Required (default: _NULL_)** <br />
Maps center position set by the given Coordinates or Address.
Also can be set the device location by setting `CenterCurrentLocationOnLoad` to `true`.
- **`CenterCurrentLocationOnLoad`: `bool { get; set; }` (default: false)** <br />
If set `true` then Maps try to detect device location by using `IGeolocationService` and center on the Map once when Map was first loaded.
**Note:** it will override <see cref="Center"/> location, but detecting geolocation is an `async` operation. It means map might be centered after some time the page rendered or location might fail!
- **`ApiKey`: `string { get; set; }` - Required** <br />
Required allows you to monitor your application's API usage in the Google Cloud Console.
- **`AnimateCenterChange`: `bool { get; set; }` (default: true)** <br />
Apply animation on Maps center change.
- **`MapType`: `GoogleMapTypes { get; set; }` (default: GoogleMapTypes.Roadmap)** <br />
Defines the type of map to construct. There are several possible maptype values, including roadmap, satellite, hybrid, and terrain.
- **`MapTypeControlOptions`: `GoogleMapTypeControlOptions { get; set; }`** <br />
Defines the type of map to construct. There are several possible maptype values, including roadmap, satellite, hybrid, and terrain.
- **`Heading`: `int { get; set; }` (default: 0)** <br />
The heading for aerial imagery in degrees measured clockwise from cardinal direction North.
Headings are snapped to the nearest available angle for which imagery is available.
- **`Tilt`: `byte { get; set; }` (default: 0)** <br />
Controls the automatic switching behavior for the angle of incidence of the map. The only allowed values are 0 and 45.
45° imagery is not available (this is the default behavior). 45° imagery is only available for satellite and hybrid map types, within some locations, and at some zoom levels.
- **`ClickableIcons`: `bool { get; set; }` (default: true)** <br />
When false, map icons are not clickable. A map icon represents a point of interest, also known as a POI. By default map icons are clickable.
- **`DisableDefaultUI`: `bool { get; set; }` (default: false)** <br />
Enables/disables all default UI buttons. May be overridden individually. Does not disable the keyboard controls, which are separately controlled.
- **`DisableDoubleClickZoom`: `bool { get; set; }` (default: false)** <br />
Enables/disables zoom and center on double click. Enabled by default.
- **`DraggableCursor`: `string? { get; set; }` (default: NULL)** <br />
The name or url of the cursor to display when mousing over a draggable map.
This property uses the css cursor attribute to change the icon.
- **`DraggingCursor`: `string? { get; set; }` (default: NULL)** <br />
The name or url of the cursor to display when the map is being dragged.
This property uses the css cursor attribute to change the icon.
- **`FullscreenControl`: `bool { get; set; }` (default: true)** <br />
The enabled/disabled state of the Fullscreen control.
- **`FullscreenControlPositon`: `GoogleMapControlPositions { get; set; }` (default: GoogleMapControlPositions.TOP_RIGHT)** <br />
The display options for the Fullscreen control.
- **`GestureHandling`: `GoogleMapGestureHandlingTypes { get; set; }` (default: GoogleMapGestureHandlingTypes.Auto)** <br />
This setting controls how the API handles gestures on the map.
- **`KeyboardShortcuts`: `bool { get; set; }` (default: true)** <br />
If false, prevents the map from being controlled by the keyboard. Keyboard shortcuts are enabled by default.
- **`MapTypeControl`: `bool { get; set; }` (default: true)** <br />
The initial enabled/disabled state of the Map type control.
- **`RotateControl`: `bool { get; set; }` (default: true)** <br />
The enabled/disabled state of the Rotate control.
- **`RotateControlOptionsPosition`: `GoogleMapControlPositions { get; set; }` (default: GoogleMapControlPositions.TOP_RIGHT)** <br />
The display options for the Rotate control.
- **`ScaleControl`: `bool { get; set; }` (default: true)** <br />
The initial enabled/disabled state of the Scale control.
- **`StreetViewControl`: `bool { get; set; }` (default: true)** <br />
The initial enabled/disabled state of the Street View Pegman control. This control is part of the default UI,
and should be set to false when displaying a map type on which the Street View road overlay should not appear (e.g. a non-Earth map type).
- **`StreetViewControlOptionsPosition`: `GoogleMapControlPositions { get; set; }` (default: GoogleMapControlPositions.RIGHT_CENTER)** <br />
The initial display options for the Street View Pegman control.
- **`Styles`: `GoogleMapTypeCustomStyle[]? { get; set; }` (default: NULL)** <br />
Styles to apply to each of the default map types. Note that for satellite/hybrid and terrain modes,
these styles will only apply to labels and geometry.

**Arbitrary HTML attributes e.g.: `tabindex="20"` will be passed to the corresponding rendered root HTML wrapper element `<div>`**.

### Events
- **`OnCurrentLocationDetected`: `EventCallback<GeolocationData>`** <br />
Callback function called when location successfully detected with `IGeolocationService`.
Device position will be supplied in the event which should be used to override `Center` parameter value.
- **`OnMapInitialized`: `EventCallback<string>`** <br />
Callback function for Google Map initialized event.
- **`OnMapClicked`: `EventCallback<GeolocationCoordinate>`** <br />
Callback function for Google Map clicked event.
- **`OnMapDoubleClicked`: `EventCallback<GeolocationCoordinate>`** <br />
Callback function for Google Map double clicked event.
- **`OnMapContextMenu`: `EventCallback<GeolocationCoordinate>`** <br />
Callback function for Google Map content menu event.
- **`OnMapMouseUp`: `EventCallback<GeolocationCoordinate>`** <br />
Callback function for Google Map mouse up event.
- **`OnMapMouseDown`: `EventCallback<GeolocationCoordinate>`** <br />
Callback function for Google Map mouse down event.
- **`OnMouseMove`: `EventCallback<GeolocationCoordinate>`** <br />
Callback function for Google Map mouse move event.
- **`OnMapMouseOver`: `EventCallback`** <br />
Callback function for Google Map mouse enter event.
- **`OnMapMouseOut`: `EventCallback<GeolocationCoordinate>`** <br />
Callback function for Google Map mouse leaving event.
- **`OnMapCenterChanged`: `EventCallback<GeolocationData>`** <br />
Callback function for Google Map Center point changed event.
**Can be used for Two-way binding: @bind-Center="{variable}" @bind-Center:event="OnMapCenterChanged".**
- **`OnMapZoomLevelChanged`: `EventCallback<byte>`** <br />
Callback function for Google Map zoom level changed event.
**Can be used for Two-way binding: @bind-ZoomLevel="{variable}" @bind-ZoomLevel:event="OnMapZoomLevelChanged".**
- **`OnMapTypeChanged`: `EventCallback<GoogleMapTypes>`** <br />
Callback function for Google Map type changed event.
**Can be used for Two-way binding: @bind-MapType="{variable}" @bind-MapType:event="OnMapTypeChanged".**
- **`OnMapHeadingChanged`: `EventCallback<int>`** <br />
Callback function for Google Map heading changed event.
**Can be used for Two-way binding: @bind-Heading="{variable}" @bind-ZoomLevel:event="OnMapHeadingChanged".**
- **`OnMapTiltChanged`: `EventCallback<byte>`** <br />
Callback function for Google Map tilt position changed event.
**Can be used for Two-way binding: @bind-Tilt="{variable}" @bind-ZoomLevel:event="OnMapTiltChanged".**
- **`OnMapBoundsChanged`: `EventCallback`** <br />
Callback function for Google Map boundaries changed event.
- **`OnMapProjectionChanged`: `EventCallback`** <br />
Callback function for Google Map projection changed event.
- **`OnMapDraggableChanged`: `EventCallback`** <br />
Callback function for Google Map draggable changed event.
- **`OnMapStreetviewChanged`: `EventCallback`** <br />
Callback function for Google Map street-view changed event.
- **`OnMapDrag`: `EventCallback<GeolocationCoordinate>`** <br />
Callback function for Google Map dragging event.
- **`OnMapInitialized`: `EventCallback<GeolocationCoordinate>`** <br />
Callback function for Google Map drag ended event.
- **`OnMapInitialized`: `EventCallback<GeolocationCoordinate>`** <br />
Callback function for Google Map drag started event.
- **`OnMapResized`: `EventCallback<Rect>`** <br />
Callback function for Google Map resized event.
- **`OnMapTilesLoaded`: `EventCallback`** <br />
Callback function for Google Map tiles loaded event.
- **`OnMapIdle`: `EventCallback`** <br />
Callback function for Google Map idle event.

### Functions
- **`CenterCurrentLocationOnMapAsync()`: `Task CenterCurrentLocationOnMapAsync()`** <br />
Starts an async operation to try to detect device location by using `IGeolocationService`.
Once operation has finished successfully `OnLocationDetected` event will be fired.
- **`DisposeAsync()`: `Task DisposeAsync()`** <br />
Component implements `IAsyncDisposable` interface Blazor framework will call it when parent removed from render tree.

### Google Places AutoComplete

### Properties

- **`ApiKey`: `string { get; set; }` - Required** <br />
Required allows you to monitor your application's API usage in the Google Cloud Console.
- **`Class`: `string { get; set; }`** <br />
Lets you add custom attributes to your autocomplete input.

### Events

- **`OnPlaceChanged`: `EventCallback<GooglePlace>`** <br />
Callback function for Google autocomplete place change listener function. Returns google place data.

### Final notice

Remember to provide your own google api key when using this library. Please check provided examples for details.

## Disclaimer

REMEMBER YOU ARE RESPONSIBLE FOR SECURING YOUR OWN GOOGLE API KEY WHEN USING THIS LIBRARY AS YOU API KEY WILL BE EXPOSED IN APPLICATION.