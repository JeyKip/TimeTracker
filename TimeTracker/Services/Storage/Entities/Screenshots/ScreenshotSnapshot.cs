using System;
using System.Collections.Generic;

namespace TimeTracker.Services.Storage
{
    /// <summary>
    /// Represents a snapshot of screenshot data to be synced with the API.
    /// </summary>
    public class ScreenshotSnapshot
    {
        
        /// <summary>
        /// Gets or sets a collection of all screenshot been made between two API syncs.
        /// </summary>
        public List<ScreenshotItem> Screenshots { get; set; } = new List<ScreenshotItem>();
    }
}