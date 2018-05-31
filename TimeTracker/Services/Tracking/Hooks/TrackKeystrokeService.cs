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

    public class TrackKeystrokeService : ITrackKeystrokeService
    {
        #region Fields and properties

        private int _keystrokesCount = 0;

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
            _keystrokesCount = 0;
        }

        public int GetHooksCount()
        {
            return _keystrokesCount;
        }

        public void TrackHook(KeystrokeModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _logger.LogDebug($"TrackHook: {entity.KeyCode}");
            _keystrokesCount++;
        }
    }
}