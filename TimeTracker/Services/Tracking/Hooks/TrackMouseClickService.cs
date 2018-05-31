using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Services.Models;

namespace TimeTracker.Services.Tracking.Hooks
{
    public interface ITrackMouseClickService : ITrackHooksService<MouseClickModel>
    {
    }

    public class TrackMouseClickService : ITrackMouseClickService
    {
        #region Fields and properties

        // maybe we should count separately left and right buttons clicks
        private int _mouseClicksCount = 0;

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
            _mouseClicksCount = 0;
        }

        public int GetHooksCount()
        {
            return _mouseClicksCount;
        }

        public void TrackHook(MouseClickModel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _logger.LogDebug($"TrackMouseHook: {Newtonsoft.Json.JsonConvert.SerializeObject(entity)}");
            _mouseClicksCount++;
        }
    }
}