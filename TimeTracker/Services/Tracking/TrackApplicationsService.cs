using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Tracking
{
    public interface ITrackApplicationsService
    {
        Task TrackInstalledApplications();
        Task TrackOpenedApplications();
    }

    public class TrackApplicationsService : ITrackApplicationsService
    {
        #region Fields and properties

        private readonly ITrackApplicationsStorageService _storageService;
        private readonly ILogger<TrackApplicationsService> _logger;

        #endregion

        #region Constructors

        public TrackApplicationsService(ITrackApplicationsStorageService storageService, ILogger<TrackApplicationsService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        #endregion

        #region ITrackApplicationsService

        public Task TrackInstalledApplications()
        {
            return Task.Run(() => { _logger.LogDebug("TrackApplicationsService - TrackInstalledApplications"); });
        }

        public Task TrackOpenedApplications()
        {
            return Task.Run(() => { _logger.LogDebug("TrackApplicationsService - TrackOpenedApplications"); });
        }

        #endregion
    }
}
