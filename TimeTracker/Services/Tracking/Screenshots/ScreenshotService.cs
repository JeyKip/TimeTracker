using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTracker.Common;
using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Tracking.Screenshots
{
    public class ScreenshotService : IScreenshotService, ITakeSnapshot<ScreenshotSnapshot>
    {
        #region Fields and properties

        private readonly ILogger<ScreenshotService> _logger;
        private ConcurrentDictionary<Guid, ScreenshotItem> _trackItems { get; set; }

        #endregion

        #region Constructors

        public ScreenshotService(ILogger<ScreenshotService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _trackItems = new ConcurrentDictionary<Guid, ScreenshotItem>();
        }

        #endregion

        #region ITrackApplicationsService

        public async Task<ResultBase> TrackAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var result = new ResultBase { Status = true };
                    var items = MakeScreenshots();
                    foreach (var item in items)
                    {
                        if (!_trackItems.TryAdd(item.Id, item))
                        {
                            result.Status = false;
                            result.ErrorMessage = $"System failed to add a screenshot item {item.Id} to dictionary";
                            _logger.LogError(result.ErrorMessage);
                        }
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    return new ResultBase
                    {
                        Status = false,
                        ErrorMessage = ex.Message
                    };
                }
            });
        }

        #endregion

        #region ITakeSnapshot

        public ScreenshotSnapshot TakeSnapshot()
        {
            var result = new ScreenshotSnapshot();
            result.Screenshots = _trackItems.Values.ToList();
            return result;
        }

        public bool ClearSnapshot(IEnumerable<Guid> ids2remove)
        {
            bool result = true;
            foreach (var id in ids2remove)
            {
                if (!_trackItems.TryRemove(id, out var removedItem))
                {
                    _logger.LogError($"System failed to remove a screenshot {id} from disctionary");
                    result = false;
                }
            }
            return result;
        }

        #endregion

        #region private helperd methods

        private IEnumerable<ScreenshotItem> MakeScreenshots() {
            // Determine the size of the "virtual screen", which includes all monitors.
            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;

            var item = new ScreenshotItem {
                Height = screenHeight,
                Width = screenWidth,
            };
            // Create a bitmap of the appropriate size to receive the screenshot.
            using (Bitmap bmp = new Bitmap(screenWidth, screenHeight))
            {
                // Draw the screenshot into our bitmap.
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
                }
                item.Image = ImageToByte(bmp);
            }

            return new List<ScreenshotItem>() { item };
        }
        private byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        #endregion

    }
}