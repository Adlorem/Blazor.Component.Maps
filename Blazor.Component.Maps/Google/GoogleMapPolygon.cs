namespace Blazor.Component.Maps;
using Color = string;
public class GoogleMapPolygon
{
    public string Id { get; set; }
    // A list of latitude and longitude pairs that make up the polygon
    public List<LatLng> Paths { get; set; }

    // The stroke color of the polygon's outline
    public Color StrokeColor { get; set; }

    // The stroke opacity of the polygon's outline
    public double StrokeOpacity { get; set; }

    // The stroke width of the polygon's outline
    public int StrokeWeight { get; set; }

    // The fill color of the polygon
    public Color FillColor { get; set; }

    // The fill opacity of the polygon
    public double FillOpacity { get; set; }

    // Determines whether the polygon is clickable
    public bool Clickable { get; set; }

    // Determines whether the polygon is editable
    public bool Editable { get; set; }

    // Determines whether the polygon is visible
    public bool Visible { get; set; }

    // The z-index of the polygon, which determines the stacking order
    public int ZIndex { get; set; }

    public GoogleMapPolygon()
    {
        Id = Guid.NewGuid().ToString();
    }
}

public class LatLng
{
    public double Lat { get; set; }
    public double Lng { get; set; }

    public LatLng(double lat, double lng)
    {
        Lat = lat;
        Lng = lng;
    }
}