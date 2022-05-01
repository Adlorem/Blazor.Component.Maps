﻿using System;
using System.Threading.Tasks;

namespace Blazor.Component.Maps
{
    /// <summary>
    /// Custom control info to add to the Map that will execute callbacks for events.
    /// </summary>
    public class GoogleMapCustomControl : GoogleMapCustomControlBase
    {
        /// <summary>
        /// Callback function called when custom control was clicked.
        /// </summary>
        public Func<string, Task>? OnClickCallback { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="htmlContent">Custom control HTML content</param>
        /// <param name="onClickCallback">Custom control on click event callback</param>
        public GoogleMapCustomControl(string htmlContent, Func<string, Task>? onClickCallback = null)
            : base(htmlContent)
        {
            OnClickCallback = onClickCallback;
        }
    }
}