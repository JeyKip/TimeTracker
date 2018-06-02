using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Properties;

namespace TimeTracker.Services.Sync
{
    public class ApiStubWrapper : ITrackApiWrapper
    {
        private ILogger<ApiStubWrapper> _logger;

        public ApiStubWrapper(ILogger<ApiStubWrapper> logger)
        {
            _logger = logger;
        }

        public async Task<SendAsyncResult> SendAsync(PushUpdatesRequest request)
        {
            return await Task.Run(() =>
            {
                var result = new SendAsyncResult
                {
                    Status = true
                };
                try
                {
                    //prepare file name
                    var dir = $"{AppDomain.CurrentDomain.BaseDirectory}\\Logs";
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    var fileName = $"dump_{DateTime.UtcNow.ToString("yyMMdd_HHmmss")}";
                    // serialize JSON to a string and then write string to a file
                    File.WriteAllText(Path.Combine(dir, fileName), JsonConvert.SerializeObject(request));

                    // example how to canvert screenshot byte array back to image
                    if (request.Screenshots != null && request.Screenshots.Screenshots.Any())
                    {
                        foreach (var screenshot in request.Screenshots.Screenshots) {
                            ImageConverter converter = new ImageConverter();
                            var img = (Image)converter.ConvertFrom(screenshot.Image);
                            img.Save($"{AppDomain.CurrentDomain.BaseDirectory}\\Logs\\{screenshot.Id.ToString()}.jpg", ImageFormat.Jpeg);
                            img.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    result.Status = false;
                    result.ErrorMessage = Resources.APIStubErrorMessage;
                }
                return result;
            });
        }
    }
}
