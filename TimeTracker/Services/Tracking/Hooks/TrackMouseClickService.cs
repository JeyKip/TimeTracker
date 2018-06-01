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
    public interface ITrackMouseClickService : ITrackHooksService<MouseClickModel>
    {
    }

    public class TrackMouseClickService : ITrackMouseClickService, ITakeSnapshot<MouseClicksSnapshot>
    {
        #region Fields and properties

        // maybe we should count separately left and right buttons clicks
        private int _mouseClicksCount = 0;
        private object _lockObject = new object();
        private readonly ILogger<TrackMouseClickService> _logger;

        #endregion

        #region Constructors

        public TrackMouseClickService(ILogger<TrackMouseClickService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        public void Clear()
        {
            lock (_lockObject)
            {
                _mouseClicksCount = 0;
            }
        }

        public void TrackHook(MouseClickModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            lock (_lockObject)
            {
                _mouseClicksCount++;
            }
        }

        public MouseClicksSnapshot TakeSnapshot()
        {
            var result = new MouseClicksSnapshot();
            var count = 0;
            lock (_lockObject)
            {
                count = _mouseClicksCount;
                Clear();
            }
            result.MouseClickCount = count;
            return result;
        }
    }
}