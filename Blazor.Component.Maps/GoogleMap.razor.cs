using Blazor.Component.Maps.Geolocation;
using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;
using System.Dynamic;

namespace Blazor.Component.Maps
{
    public partial class GoogleMap
    {
        /// <summary>
        /// If set `true` then Maps try to detect device location by using <see cref="IGeolocationService"/> and center on the Map once when Map was first loaded.
        /// Note: it will override <see cref="Center"/> location, but detecting geolocation is an `async` operation. It means map might be centered after some time the page rendered or location might fail!
        /// </summary>
        [Parameter] public bool CenterCurrentLocationOnLoad { get; set; } = false;

        /// <summary>
        /// Required allows you to monitor your application's API usage in the Google Cloud Console.
        /// </summary>
        [Parameter] public string ApiKey { get; set; }

        //Events
        /// <summary>
        /// Callback function for Google Map initialized event.
        /// </summary>
        [Parameter] public EventCallback<string> OnMapInitialized { get; set; }

        /// <summary>
        /// Callback function for Google Map clicked event.
        /// </summary>
        [Parameter] public EventCallback<GeolocationCoordinate> OnMapClicked { get; set; }
        /// <summary>
        /// Callback function for Google Map double clicked event.
        /// </summary>
        [Parameter] public EventCallback<GeolocationCoordinate> OnMapDoubleClicked { get; set; }
        /// <summary>
        /// Callback function for Google Map content menu event.
        /// </summary>
        [Parameter] public EventCallback<GeolocationCoordinate> OnMapContextMenu { get; set; }
        /// <summary>
        /// Callback function for Google Map mouse up event.
        /// </summary>
        [Parameter] public EventCallback<GeolocationCoordinate> OnMapMouseUp { get; set; }
        /// <summary>
        /// Callback function for Google Map mouse down event.
        /// </summary>
        [Parameter] public EventCallback<GeolocationCoordinate> OnMapMouseDown { get; set; }
        /// <summary>
        /// Callback function for Google Map mouse move event.
        /// </summary>
        [Parameter] public EventCallback<GeolocationCoordinate> OnMouseMove { get; set; }
        /// <summary>
        /// Callback function for Google Map mouse enter event.
        /// </summary>
        [Parameter] public EventCallback OnMapMouseOver { get; set; }
        /// <summary>
        /// Callback function for Google Map mouse leaving event.
        /// </summary>
        [Parameter] public EventCallback<GeolocationCoordinate> OnMapMouseOut { get; set; }

        /// <summary>
        /// Callback function for Google Map Center point changed event.
        /// Can be used for Two-way binding: @bind-Center="{variable}" @bind-Center:event="OnMapCenterChanged".
        /// </summary>
        [Parameter] public EventCallback<GeolocationData> OnMapCenterChanged { get; set; }
        /// <summary>
        /// Callback function for Google Map zoom level changed event.
        /// Can be used for Two-way binding: @bind-Zoom="{variable}" @bind-Zoom:event="OnMapZoomLevelChanged".
        /// </summary>
        [Parameter] public EventCallback<byte> OnMapZoomLevelChanged { get; set; }
        /// <summary>
        /// Callback function for Google Map type changed event.
        /// Can be used for Two-way binding: @bind-MapType="{variable}" @bind-MapType:event="OnMapTypeChanged".
        /// </summary>
        [Parameter] public EventCallback<GoogleMapTypes> OnMapTypeChanged { get; set; }
        /// <summary>
        /// Callback function for Google Map heading changed event.
        /// Can be used for Two-way binding: @bind-Heading="{variable}" @bind-ZoomLevel:event="OnMapHeadingChanged".
        /// </summary>
        [Parameter] public EventCallback<int> OnMapHeadingChanged { get; set; }
        /// <summary>
        /// Callback function for Google Map tilt position changed event.
        /// Can be used for Two-way binding: @bind-Tilt="{variable}" @bind-ZoomLevel:event="OnMapTiltChanged".
        /// </summary>
        [Parameter] public EventCallback<byte> OnMapTiltChanged { get; set; }
        /// <summary>
        /// Callback function for Google Map boundaries changed event.
        /// </summary>
        [Parameter] public EventCallback OnMapBoundsChanged { get; set; }
        /// <summary>
        /// Callback function for Google Map projection changed event.
        /// </summary>
        [Parameter] public EventCallback OnMapProjectionChanged { get; set; }
        /// <summary>
        /// Callback function for Google Map draggable changed event.
        /// </summary>
        [Parameter] public EventCallback OnMapDraggableChanged { get; set; }
        /// <summary>
        /// Callback function for Google Map street-view changed event.
        /// </summary>
        [Parameter] public EventCallback OnMapStreetviewChanged { get; set; }

        /// <summary>
        /// Callback function for Google Map dragging event.
        /// </summary>
        [Parameter] public EventCallback<GeolocationCoordinate> OnMapDrag { get; set; }
        /// <summary>
        /// Callback function for Google Map drag ended event.
        /// </summary>
        [Parameter] public EventCallback<GeolocationCoordinate> OnMapDragEnd { get; set; }
        /// <summary>
        /// Callback function for Google Map drag started event.
        /// </summary>
        [Parameter] public EventCallback<GeolocationCoordinate> OnMapDragStart { get; set; }

        /// <summary>
        /// Callback function for Google Map resized event.
        /// </summary>
        [Parameter] public EventCallback<Rect> OnMapResized { get; set; }
        /// <summary>
        /// Callback function for Google Map tiles loaded event.
        /// </summary>
        [Parameter] public EventCallback OnMapTilesLoaded { get; set; }
        /// <summary>
        /// Callback function for Google Map idle event.
        /// </summary>
        [Parameter] public EventCallback OnMapIdle { get; set; }

        /// <summary>
        /// Callback function called when location successfully detected with <see cref="IGeolocationService"/>.
        /// Device positon will be supplied in the event which should be used to override <see cref="Center"/> parameter value.
        /// </summary>
        [Parameter] public EventCallback<GeolocationData> OnCurrentLocationDetected { get; set; }
        /// <summary>
        /// Blazor capture for any unmatched HTML attributes.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> AllOtherAttributes { get; set; }
        //Settings only for initialization time.
        /// <summary>
        /// Color used for the background of the Map div. This color will be visible when tiles have not yet loaded as the user pans.
        /// This option can only be set when the map is initialized.
        /// </summary>
        [Parameter] public string? BackgroundColor { get; set; }
        /// <summary>
        /// Size in pixels of the controls appearing on the map. This value must be supplied directly when creating the Map.
        /// </summary>
        [Parameter] public int ControlSize { get; set; }
        /// <summary>
        /// Custom controls to add to the Map that will execute callbacks for events.
        /// This option can only be set when the map is initialized. Use <see cref="OnInitialized"/> method to set it up.
        /// </summary>
        [Parameter] public IEnumerable<GoogleMapCustomControl>? CustomControls { get; set; }
        private ObservableRangeCollection<GoogleMapMarker>? _markers;
        /// <summary>
        /// MarkerOptions object used to define the properties that can be set on a Marker.
        /// ObservableCollection can be initialized only once! Add or remove items to the collection (use `OnMapInitialized` event or user interactions) the change marker properties (Marker properties value changes not detected).
        /// </summary>
        [Parameter]
        public ObservableRangeCollection<GoogleMapMarker>? Markers
        {
            get => _markers;
            set
            {
                if (_markers is null && value is not null) //Subscribe to events only once!
                {
                    _markers = value;
                    _markers.CollectionChanged += MarkersChanges;
                }
            }
        }
        private void MarkersChanges(object? sender, NotifyCollectionChangedEventArgs e)
        {
            InvokeAsync(async () => await _mapService.CreateMarkersAsync(e.NewItems?.Cast<GoogleMapMarker>(), _markers?.Cast<GoogleMapMarker>()));
        }

        private byte _zoomLevel = 3;
        /// <summary>
        /// Defines the zoom level of the map, which determines the magnification level of the map.
        /// </summary>
        [Parameter]
        public byte Zoom
        {
            get => _zoomLevel;
            set
            {
                if (_mapInitialized && value != _zoomLevel)
                {
                    _zoomLevel = value;
                    InvokeAsync(async () => await _mapService.SetZoomAsync(_zoomLevel));
                }
            }
        }

        private bool _zoomControl = true;
        /// <summary>
        /// The enabled/disabled state of the Zoom control.
        /// </summary>
        [Parameter]
        public bool ZoomControl
        {
            get => _zoomControl;
            set
            {
                if (_mapInitialized && value != _zoomControl)
                {
                    _zoomControl = value;

                    dynamic options = new ExpandoObject();
                    options.zoomControl = _zoomControl;
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private GoogleMapControlPositions _zoomControlOptionsPosition = GoogleMapControlPositions.BOTTOM_RIGHT;
        /// <summary>
        /// The display options for the Zoom control.
        /// </summary>
        [Parameter]
        public GoogleMapControlPositions ZoomControlOptionsPosition
        {
            get => _zoomControlOptionsPosition;
            set
            {
                if (_mapInitialized && value != _zoomControlOptionsPosition)
                {
                    _zoomControlOptionsPosition = value;

                    dynamic options = new ExpandoObject();
                    options.zoomControlOptions = new { position = _zoomControlOptionsPosition };
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private byte? _maxZoom = null;
        /// <summary>
        /// The maximum zoom level which will be displayed on the map. If omitted, or set to null,
        /// the maximum zoom from the current map type is used instead.
        /// </summary>
        [Parameter]
        public byte? MaxZoom
        {
            get => _maxZoom;
            set
            {
                if (_mapInitialized && value != _maxZoom)
                {
                    _maxZoom = value;

                    dynamic options = new ExpandoObject();
                    options.maxZoom = _maxZoom;
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private byte? _minZoom = null;
        /// <summary>
        /// The minimum zoom level which will be displayed on the map. If omitted, or set to null,
        /// the minimum zoom from the current map type is used instead.
        /// </summary>
        [Parameter]
        public byte? MinZoom
        {
            get => _minZoom;
            set
            {
                if (_mapInitialized && value != _minZoom)
                {
                    _minZoom = value;

                    dynamic options = new ExpandoObject();
                    options.minZoom = _minZoom;
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        //Size
        private int? _width;
        /// <summary>
        /// Maps image Width in px.
        /// </summary>
        [Parameter]
        public int? Width
        {
            set
            {
                if (value != null && value != _width)
                {
                    _width = value;
                    _containerWidth = $"{_width}px";

                    if (_mapInitialized)
                    {
                        InvokeAsync(async () => await _mapService.ResizeMapAsync());
                    }
                }
            }
        }
        private int? _height;
        /// <summary>
        /// Maps image Height in px.
        /// </summary>
        [Parameter]
        public int? Height
        {
            set
            {
                if (value != null && value != _height)
                {
                    _height = value.Value;
                    _containerHeight = $"{_height}px";

                    if (_mapInitialized)
                    {
                        InvokeAsync(async () => await _mapService.ResizeMapAsync());
                    }
                }
            }
        }

        private GeolocationData? _center;
        /// <summary>
        /// Maps center position set by the given Coordinates or Address.
        /// Also can be set the divece location by setting <see cref="CenterCurrentLocationOnLoad"/> to `true`.
        /// </summary>
        [Parameter]
        public GeolocationData? Center
        {
            get => _center;
            set
            {
                if (_mapInitialized && !_isDragging && _center?.ToString() != value?.ToString())
                {
                    _center = value;
                    InvokeAsync(async () =>
                    {
                        if (Center?.HasCoordinates ?? false)
                        {
                            if (AnimateCenterChange)
                                await _mapService.PanToAsync(Center.Latitude.Value, Center.Longitude.Value);
                            else
                                await _mapService.SetCenterAsync(Center.Latitude.Value, Center.Longitude.Value);
                        }
                        else if (!string.IsNullOrWhiteSpace(Center?.Address))
                        {
                            if (AnimateCenterChange)
                                await _mapService.PanToAsync(Center.Address);
                            else
                                await _mapService.SetCenterAsync(Center.Address);
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Apply animation on Maps center change.
        /// </summary>
        [Parameter] public bool AnimateCenterChange { get; set; } = true;

        private GoogleMapTypes _mapType = GoogleMapTypes.Roadmap;
        /// <summary>
        /// Defines the type of map to construct. There are several possible maptype values, including roadmap, satellite, hybrid, and terrain.
        /// </summary>
        [Parameter]
        public GoogleMapTypes MapType
        {
            get => _mapType;
            set
            {
                if (_mapInitialized && _mapType != value)
                {
                    _mapType = value;
                    InvokeAsync(async () => await _mapService.SetMapTypeAsync(_mapType));
                }
            }
        }

        private GoogleMapTypeControlOptions _mapTypeControlOptions = new GoogleMapTypeControlOptions();
        /// <summary>
        /// Defines the type of map to construct. There are several possible maptype values, including roadmap, satellite, hybrid, and terrain.
        /// </summary>
        [Parameter]
        public GoogleMapTypeControlOptions MapTypeControlOptions
        {
            get => _mapTypeControlOptions;
            set
            {
                if (_mapInitialized && _mapTypeControlOptions != value)
                {
                    _mapTypeControlOptions = value;

                    dynamic options = new ExpandoObject();
                    options.mapTypeControlOptions = new
                    {
                        mapTypeIds = _mapTypeControlOptions?.MapTypeIds?.Select(s => s.ToString().ToLower()).ToArray(),
                        position = _mapTypeControlOptions?.Position,
                        style = _mapTypeControlOptions?.MapTypeControlStyle
                    };
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private int _heading;
        /// <summary>
        /// The heading for aerial imagery in degrees measured clockwise from cardinal direction North.
        /// Headings are snapped to the nearest available angle for which imagery is available.
        /// </summary>
        [Parameter]
        public int Heading
        {
            get => _heading;
            set
            {
                if (_mapInitialized && value != _heading)
                {
                    _heading = value;
                    InvokeAsync(async () => await _mapService.SetHeadingAsync(_heading));
                }
            }
        }

        private byte _tilt;
        /// <summary>
        /// Controls the automatic switching behavior for the angle of incidence of the map. The only allowed values are 0 and 45.
        /// 45° imagery is not available (this is the default behavior). 45° imagery is only available for satellite and hybrid map types, within some locations, and at some zoom levels.
        /// </summary>
        [Parameter]
        public byte Tilt
        {
            get => _tilt;
            set
            {
                if (_mapInitialized && value != _tilt)
                {
                    _tilt = value;
                    InvokeAsync(async () => await _mapService.SetTiltAsync(_tilt));
                }
            }
        }

        private bool _clickableIcons = true;
        /// <summary>
        /// When false, map icons are not clickable. A map icon represents a point of interest, also known as a POI.
        /// By default map icons are clickable.
        /// </summary>
        [Parameter]
        public bool ClickableIcons
        {
            get => _clickableIcons;
            set
            {
                if (_mapInitialized && value != _clickableIcons)
                {
                    _clickableIcons = value;
                    InvokeAsync(async () => await _mapService.SetClickableIconsAsync(_clickableIcons));
                }
            }
        }

        private bool _disableDefaultUI = false;
        /// <summary>
        /// Enables/disables all default UI buttons. May be overridden individually.
        /// Does not disable the keyboard controls, which are separately controlled.
        /// </summary>
        [Parameter]
        public bool DisableDefaultUI
        {
            get => _disableDefaultUI;
            set
            {
                if (_mapInitialized && value != _disableDefaultUI)
                {
                    _disableDefaultUI = value;

                    dynamic options = new ExpandoObject();
                    options.disableDefaultUI = _disableDefaultUI;
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private bool _disableDoubleClickZoom = false;
        /// <summary>
        /// Enables/disables zoom and center on double click. Enabled by default.
        /// </summary>
        [Parameter]
        public bool DisableDoubleClickZoom
        {
            get => _disableDoubleClickZoom;
            set
            {
                if (_mapInitialized && value != _disableDoubleClickZoom)
                {
                    _disableDoubleClickZoom = value;

                    dynamic options = new ExpandoObject();
                    options.disableDoubleClickZoom = _disableDoubleClickZoom;
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private string? _draggableCursor = null;
        /// <summary>
        /// The name or url of the cursor to display when mousing over a draggable map.
        /// This property uses the css cursor attribute to change the icon.
        /// </summary>
        [Parameter]
        public string? DraggableCursor
        {
            get => _draggableCursor;
            set
            {
                if (_mapInitialized && value != _draggableCursor)
                {
                    _draggableCursor = value;

                    dynamic options = new ExpandoObject();
                    options.draggableCursor = _draggableCursor;
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private string? _draggingCursor = null;
        /// <summary>
        /// The name or url of the cursor to display when the map is being dragged.
        /// This property uses the css cursor attribute to change the icon.
        /// </summary>
        [Parameter]
        public string? DraggingCursor
        {
            get => _draggingCursor;
            set
            {
                if (_mapInitialized && value != _draggingCursor)
                {
                    _draggingCursor = value;

                    dynamic options = new ExpandoObject();
                    options.draggingCursor = _draggingCursor;
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private bool _fullscreenControl = true;
        /// <summary>
        /// The enabled/disabled state of the Fullscreen control.
        /// </summary>
        [Parameter]
        public bool FullscreenControl
        {
            get => _fullscreenControl;
            set
            {
                if (_mapInitialized && value != _fullscreenControl)
                {
                    _fullscreenControl = value;

                    dynamic options = new ExpandoObject();
                    options.fullscreenControl = _fullscreenControl;
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private GoogleMapControlPositions _fullscreenControlPosition = GoogleMapControlPositions.TOP_RIGHT;
        /// <summary>
        /// The display options for the Fullscreen control.
        /// </summary>
        [Parameter]
        public GoogleMapControlPositions FullscreenControlPositon
        {
            get => _fullscreenControlPosition;
            set
            {
                if (_mapInitialized && value != _fullscreenControlPosition)
                {
                    _fullscreenControlPosition = value;

                    dynamic options = new ExpandoObject();
                    options.fullscreenControlOptions = new { position = _fullscreenControlPosition };
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private GoogleMapGestureHandlingTypes _gestureHandling = GoogleMapGestureHandlingTypes.Auto;
        /// <summary>
        /// This setting controls how the API handles gestures on the map.
        /// </summary>
        [Parameter]
        public GoogleMapGestureHandlingTypes GestureHandling
        {
            get => _gestureHandling;
            set
            {
                if (_mapInitialized && value != _gestureHandling)
                {
                    _gestureHandling = value;

                    dynamic options = new ExpandoObject();
                    options.gestureHandling = _gestureHandling.ToString().ToLower();
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private bool _keyboardShortcuts = true;
        /// <summary>
        /// If false, prevents the map from being controlled by the keyboard. Keyboard shortcuts are enabled by default.
        /// </summary>
        [Parameter]
        public bool KeyboardShortcuts
        {
            get => _keyboardShortcuts;
            set
            {
                if (_mapInitialized && value != _keyboardShortcuts)
                {
                    _keyboardShortcuts = value;

                    dynamic options = new ExpandoObject();
                    options.keyboardShortcuts = _keyboardShortcuts;
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private bool _mapTypeControl = true;
        /// <summary>
        /// The initial enabled/disabled state of the Map type control.
        /// </summary>
        [Parameter]
        public bool MapTypeControl
        {
            get => _mapTypeControl;
            set
            {
                if (_mapInitialized && value != _mapTypeControl)
                {
                    _mapTypeControl = value;

                    dynamic options = new ExpandoObject();
                    options.mapTypeControl = _mapTypeControl;
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private bool _rotateControl = true;
        /// <summary>
        /// The enabled/disabled state of the Rotate control.
        /// </summary>
        [Parameter]
        public bool RotateControl
        {
            get => _rotateControl;
            set
            {
                if (_mapInitialized && value != _rotateControl)
                {
                    _rotateControl = value;

                    dynamic options = new ExpandoObject();
                    options.rotateControl = _rotateControl;
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private GoogleMapControlPositions _rotateControlOptionsPosition = GoogleMapControlPositions.TOP_RIGHT;
        /// <summary>
        /// The display options for the Rotate control.
        /// </summary>
        [Parameter]
        public GoogleMapControlPositions RotateControlOptionsPosition
        {
            get => _rotateControlOptionsPosition;
            set
            {
                if (_mapInitialized && value != _rotateControlOptionsPosition)
                {
                    _rotateControlOptionsPosition = value;

                    dynamic options = new ExpandoObject();
                    options.rotateControlOptions = new { position = _rotateControlOptionsPosition };
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private bool _scaleControl = true;
        /// <summary>
        /// The initial enabled/disabled state of the Scale control.
        /// </summary>
        [Parameter]
        public bool ScaleControl
        {
            get => _scaleControl;
            set
            {
                if (_mapInitialized && value != _scaleControl)
                {
                    _scaleControl = value;

                    dynamic options = new ExpandoObject();
                    options.rotateControl = _scaleControl;
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private bool _streetViewControl = true;
        /// <summary>
        /// The initial enabled/disabled state of the Street View Pegman control. This control is part of the default UI,
        /// and should be set to false when displaying a map type on which the Street View road overlay should not appear (e.g. a non-Earth map type).
        /// </summary>
        [Parameter]
        public bool StreetViewControl
        {
            get => _streetViewControl;
            set
            {
                if (_mapInitialized && value != _streetViewControl)
                {
                    _streetViewControl = value;

                    dynamic options = new ExpandoObject();
                    options.streetViewControl = _streetViewControl;
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private GoogleMapControlPositions _streetViewControlOptionsPosition = GoogleMapControlPositions.RIGHT_CENTER;
        /// <summary>
        /// The initial display options for the Street View Pegman control.
        /// </summary>
        [Parameter]
        public GoogleMapControlPositions StreetViewControlOptionsPosition
        {
            get => _streetViewControlOptionsPosition;
            set
            {
                if (_mapInitialized && value != _streetViewControlOptionsPosition)
                {
                    _streetViewControlOptionsPosition = value;

                    dynamic options = new ExpandoObject();
                    options.streetViewControlOptions = new { position = _streetViewControlOptionsPosition };
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private GoogleMapTypeCustomStyle[]? _styles;
        /// <summary>
        /// Styles to apply to each of the default map types. Note that for satellite/hybrid and terrain modes,
        /// these styles will only apply to labels and geometry.
        /// </summary>
        [Parameter]
        public GoogleMapTypeCustomStyle[]? Styles
        {
            get => _styles;
            set
            {
                if (_mapInitialized && value != _styles)
                {
                    _styles = value;

                    dynamic options = new ExpandoObject();
                    options.styles = _styles?.Select(s =>
                    {
                        dynamic obj = new ExpandoObject();
                        obj.stylers = s.Stylers;
                        obj.elementType = s.ElementType;
                        obj.featureType = s.FeatureType;

                        return obj;
                    }).ToArray();
                    InvokeAsync(async () => await _mapService.SetOptionsAsync(options));
                }
            }
        }

        private bool _usePlacesLibrary;
        /// <summary>
        /// Aplies places librarty to default google javascript api.
        /// </summary>
        [Parameter]
        public bool UsePlacesLibrary
        {
            get => _usePlacesLibrary;
            set
            {
                _usePlacesLibrary = value;
            }
        }
        /// <summary>
        /// Sets map center to given coordinates
        /// </summary>
        [Parameter]
        public EventCallback<GeolocationData> SetCenter { get; set; }
             
        private string _mapContainerId = $"map_{Guid.NewGuid().ToString("n")}";
        private string _containerWidth = "100%";
        private string _containerHeight = "100%";
        /// <summary>
        /// Map HTML container Id. It can be used when multiple Maps added to one page.
        /// </summary>
        public string MapId => _mapContainerId;

        private ElementReference _jsMap;
        /// <summary>
        /// Exposes a Blazor <see cref="ElementReference"/> of the wrapped around HTML element. It can be used e.g. for JS interop, etc.
        /// </summary>
        public ElementReference InnerElementReference => _jsMap;

        private bool _mapInitialized = false;
        private bool _isDragging = false;

        protected override async Task OnInitializedAsync()
        {
            {
                await _mapService.InitMapAsync(apiKey: ApiKey,
                    mapContainerId: _mapContainerId,
                    backgroundColor: BackgroundColor,
                    controlSize: ControlSize,
                    usePlacesLibrary: UsePlacesLibrary,
                    mapInitializedCallback: async (mapId) =>
                    {
                        if (CenterCurrentLocationOnLoad)
                        {
                            await CenterCurrentLocationOnMapAsync();
                        }
                        else
                        {
                            await _mapService.SetCenterAsync(52.519325, 13.392709);
                            //Center = new GeolocationData(52.519325, 13.392709);
                        }

                        _mapInitialized = true;

                        await _mapService.SetZoomAsync(Zoom); // set default zoom level

                        if (CustomControls?.Any() ?? false)
                        {
                            await _mapService.CreateCustomControlsAsync(CustomControls);
                        }

                        if (OnMapInitialized.HasDelegate)
                        {
                            await OnMapInitialized.InvokeAsync(_mapContainerId);
                        }
                    },
                    mapClickedCallback: async (coords) =>
                    {
                        if (OnMapClicked.HasDelegate)
                        {
                            await OnMapClicked.InvokeAsync(coords);
                        }
                    },
                    mapDoubleClickedCallback: async (coords) =>
                    {
                        if (OnMapDoubleClicked.HasDelegate)
                        {
                            await OnMapDoubleClicked.InvokeAsync(coords);
                        }
                    },
                    mapContextMenuCallback: async (coords) =>
                    {
                        if (OnMapContextMenu.HasDelegate)
                        {
                            await OnMapContextMenu.InvokeAsync(coords);
                        }
                    },
                    mapMouseUpCallback: async (coords) =>
                    {
                        if (OnMapMouseUp.HasDelegate)
                        {
                            await OnMapMouseUp.InvokeAsync(coords);
                        }
                    },
                    mapMouseDownCallback: async (coords) =>
                    {
                        if (OnMapMouseDown.HasDelegate)
                        {
                            await OnMapMouseDown.InvokeAsync(coords);
                        }
                    },
                    mapMouseMoveCallback: async (coords) =>
                    {
                        if (OnMouseMove.HasDelegate)
                        {
                            await OnMouseMove.InvokeAsync(coords);
                        }
                    },
                    mapMouseOverCallback: async () =>
                    {
                        if (OnMapMouseOver.HasDelegate)
                        {
                            await OnMapMouseOver.InvokeAsync();
                        }
                    },
                    mapMouseOutCallback: async () =>
                    {
                        if (OnMapMouseOut.HasDelegate)
                        {
                            await OnMapMouseOut.InvokeAsync();
                        }
                    },
                    mapCenterChangedCallback: async (coords) =>
                    {
                        if (_center?.ToString() == coords?.ToString())
                            return;

                        _center = new GeolocationData(coords?.Latitude, coords?.Longitude);
                        if (OnMapCenterChanged.HasDelegate)
                        {
                            await OnMapCenterChanged.InvokeAsync(_center);
                        }
                    },
                    mapZoomChangedCallback: async (zoom) =>
                    {
                        if (_zoomLevel == zoom)
                            return;

                        _zoomLevel = zoom;
                        if (OnMapZoomLevelChanged.HasDelegate)
                        {
                            await OnMapZoomLevelChanged.InvokeAsync(zoom);
                        }
                    },
                    mapTypeChangedCallback: async (type) =>
                    {
                        if (_mapType == type)
                            return;

                        _mapType = type;
                        if (OnMapTypeChanged.HasDelegate)
                        {
                            await OnMapTypeChanged.InvokeAsync(type);
                        }
                    },
                    mapHeadingChangedCallback: async (heading) =>
                    {
                        if (_heading == heading)
                            return;

                        _heading = heading;
                        if (OnMapHeadingChanged.HasDelegate)
                        {
                            await OnMapHeadingChanged.InvokeAsync(heading);
                        }
                    },
                    mapTiltChangedCallback: async (tilt) =>
                    {
                        if (_tilt == tilt)
                            return;

                        _tilt = tilt;
                        if (OnMapTiltChanged.HasDelegate)
                        {
                            await OnMapTiltChanged.InvokeAsync(tilt);
                        }
                    },
                    mapBoundsChangedCallback: async () =>
                    {
                        if (OnMapBoundsChanged.HasDelegate)
                        {
                            await OnMapBoundsChanged.InvokeAsync();
                        }
                    },
                    mapProjectionChangedCallback: async () =>
                    {
                        if (OnMapProjectionChanged.HasDelegate)
                        {
                            await OnMapProjectionChanged.InvokeAsync();
                        }
                    },
                    mapDraggableChangedCallback: async () =>
                    {

                        if (OnMapDraggableChanged.HasDelegate)
                        {
                            await OnMapDraggableChanged.InvokeAsync();
                        }
                    },
                    mapStreetviewChangedCallback: async () =>
                    {
                        if (OnMapStreetviewChanged.HasDelegate)
                        {
                            await OnMapStreetviewChanged.InvokeAsync();
                        }
                    },
                    mapDragCallback: async (coords) =>
                    {
                        if (OnMapDrag.HasDelegate)
                        {
                            await OnMapDrag.InvokeAsync(coords);
                        }
                    },
                    mapDragEndCallback: async (coords) =>
                    {
                        _isDragging = false;

                        if (OnMapDragEnd.HasDelegate)
                        {
                            await OnMapDragEnd.InvokeAsync(coords);
                        }
                    },
                    mapDragStartCallback: async (coords) =>
                    {
                        _isDragging = true;

                        if (OnMapDragStart.HasDelegate)
                        {
                            await OnMapDragStart.InvokeAsync(coords);
                        }
                    },
                    mapResizedCallback: async (rect) =>
                    {
                        if (OnMapResized.HasDelegate)
                        {
                            await OnMapResized.InvokeAsync(rect);
                        }
                    },
                    mapTilesLoadedCallback: async () =>
                    {
                        if (OnMapTilesLoaded.HasDelegate)
                        {
                            await OnMapTilesLoaded.InvokeAsync();
                        }
                    },
                    mapIdleCallback: async () =>
                    {
                        if (OnMapIdle.HasDelegate)
                        {
                            await OnMapIdle.InvokeAsync();
                        }
                    });
            }
        }


        /// <summary>
        /// Starts an async operation to try to detect device location by using <see cref="IGeolocationService"/>.
        /// Once operation has finished successfully <see cref="OnLocationDetected"/> event will be fired.
        /// </summary>
        /// <returns>Async task</returns>
        public async Task CenterCurrentLocationOnMapAsync()
        {
            await _geolocationService.GetCurrentPositionAsync(LocationResult, false, TimeSpan.FromSeconds(10));
        }
        private async Task LocationResult(GeolocationResult pos)
        {
            if (pos?.IsSuccess ?? false)
            {
                await _mapService.SetCenterAsync(pos.Coordinates.Latitude, pos.Coordinates.Longitude);
                Center = new GeolocationData(pos.Coordinates.Latitude, pos.Coordinates.Longitude);

                if (OnCurrentLocationDetected.HasDelegate)
                {
                    await OnCurrentLocationDetected.InvokeAsync(Center);
                }
            }
        }

        /// <summary>
        /// Sets map center to given geolocation data.
        /// </summary>
        /// <param name="coordinates"></param>
        private async Task CenterLocationOnMap(GeolocationData coordinates)
        {
            if (_mapInitialized && !_isDragging && coordinates != null && coordinates.HasCoordinates)
            {
                if (Center != null)
                {
                    if (AnimateCenterChange)
                        await _mapService.PanToAsync(coordinates.Latitude.Value, coordinates.Longitude.Value);
                    else
                        await _mapService.SetCenterAsync(coordinates.Latitude.Value, coordinates.Longitude.Value);
                }
                else if (!string.IsNullOrWhiteSpace(coordinates?.Address))
                {
                    if (AnimateCenterChange)
                        await _mapService.PanToAsync(coordinates.Address);
                    else
                        await _mapService.SetCenterAsync(coordinates.Address);
                }
               
                Center = coordinates;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_geolocationService is not null)
            {
                await _geolocationService.DisposeAsync();
            }

            if (_mapService is not null)
            {
                await _mapService.DisposeAsync();
            }

            if (_markers is not null)
            {
                _markers.CollectionChanged -= MarkersChanges;
            }
        }
    }
}