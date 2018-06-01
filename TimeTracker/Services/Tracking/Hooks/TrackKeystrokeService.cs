using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Services.Models;
using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Tracking.Hooks
{
    public interface ITrackKeystrokeService : ITrackHooksService<KeystrokeModel>
    {
    }

    public class TrackKeystrokeService : ITrackKeystrokeService, ITakeSnapshot<KeyboardClicksSnapshot>
    {
        #region Fields and properties

        private int _keystrokesCount = 0;
        private object _lockObject = new object();
        private readonly ILogger<TrackKeystrokeService> _logger;

        #endregion

        #region Constructors

        public TrackKeystrokeService(ILogger<TrackKeystrokeService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        public void Clear()
        {
            lock (_lockObject)
            {
                _keystrokesCount = 0;
            }
        }

        public void TrackHook(KeystrokeModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            lock (_lockObject)
            {
                _keystrokesCount++;
            }
        }

        public KeyboardClicksSnapshot TakeSnapshot()
        {
            var result = new KeyboardClicksSnapshot();
            var count = 0;
            lock (_lockObject)
            {
                count = _keystrokesCount;
                Clear();
            }
            result.PressButtonCount = count;
            return result;
        }
    }
}