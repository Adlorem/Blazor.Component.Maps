﻿using System.Globalization;

namespace Blazor.Component.Maps
{
    /// <summary>
    /// Represents a Geolocation Coordinate.
    /// </summary>
    public class GeolocationCoordinate
    {
        /// <summary>
        /// Representing the latitude of the position in decimal degrees.
        /// </summary>
        public double? Latitude { get; init; }
        /// <summary>
        /// Represents the longitude of a geographical position, specified in decimal degrees.
        /// </summary>
        public double? Longitude { get; init; }

        /// <summary>
        /// Checks to <see cref="Latitude"/> and <see cref="Longitude"/> are defined.
        /// </summary>
        public bool HasCoordinates => Latitude.HasValue && Longitude.HasValue;

        public GeolocationCoordinate(double? latitude, double? longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Formats coordinates to Maps specific string or return Empty.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return HasCoordinates
                ? $"{Latitude.Value.ToString(CultureInfo.InvariantCulture)},{Longitude.Value.ToString(CultureInfo.InvariantCulture)}"
                : string.Empty;
        }
    }
}