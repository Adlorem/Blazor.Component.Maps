﻿using System.Dynamic;

namespace Blazor.Component.Maps
{
    /// <summary>
    /// The MapTypeStyle is a collection of selectors and stylers that define how the map should be styled. 
    /// Selectors specify the map features and/or elements that should be affected, and stylers specify how those 
    /// features and elements should be modified.
    /// </summary>
    public class GoogleMapTypeCustomStyle
    {
        /// <summary>
        /// The style rules to apply to the selected map features and elements. 
        /// The rules are applied in the order that you specify in this array.
        /// </summary>
        public ExpandoObject[]? Stylers { get; set; }

        /// <summary>
        /// The element to which a styler should be applied. An element is a visual aspect of a feature on the map. 
        /// Example: a label, an icon, the stroke or fill applied to the geometry, and more. Optional. 
        /// If elementType is not specified, the value is assumed to be 'all'.
        /// </summary>
        public string ElementType { get; set; } = string.Empty;

        /// <summary>
        /// The feature, or group of features, to which a styler should be applied. Optional. 
        /// If featureType is not specified, the value is assumed to be 'all'. 
        /// </summary>
        public string FeatureType { get; set; } = string.Empty;
    }
}