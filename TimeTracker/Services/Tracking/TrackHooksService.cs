using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Tracking
{
    public interface ITrackHooksService
    {
        Task TrackMouseClicks();
        Task TrackKeyboardClicks();
    }

    public class TrackHooksService : ITrackHooksService
    {
        #region Fields and properties

        private readonly ITrackHooksStorageService _storageService;
        private readonly ILogger<TrackHooksService> _logger;

        #endregion

        #region Constructors

        public TrackHooksService(ITrackHooksStorageService storageService, ILogger<TrackHooksService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        #endregion

        #region ITrackHooksService

        public Task TrackKeyboardClicks()
        {
            return Task.Run(() => { _logger.LogDebug("TrackHooksService - TrackKeyboardClicks"); });
        }

        public Task TrackMouseClicks()
        {
            return Task.Run(() => { _logger.LogDebug("TrackHooksService - TrackMouseClicks"); });
        }

        #endregion
    }
}