using System;

namespace TimeTracker.Services.Storage
{
    /// <summary>
    /// Represents a info about screenshot.
    /// </summary>
    public class ScreenshotItem
    {
        /// <summary>
        /// Gets or sets an unique identifier of the screenshot.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Gets or sets a timestamp when screenshot has been made.
        /// </summary>
        public DateTime CheckDate { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// Gets or sets a width of the screen resolution.
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets a height of the screen resolution.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets a screenshot as byte array.
        /// </summary>
        public byte[] Image { get; set; }
    }
}
