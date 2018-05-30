using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Services.Sync;
using TimeTracker.Services.Tracking;

namespace TimeTracker.Services
{
    public interface ITaskRunner
    {
        Task Start();
        Task Stop();
    }

    public class TaskRunner : ITaskRunner
    {
        #region Fields and properties

        private readonly ITrackApplicationsService _trackApplicationsService;
        private readonly ITrackHooksService _trackHooksService;
        private readonly ISyncService _syncService;
        private readonly ILogger<TaskRunner> _logger;
        private Task<PushUpdatesResult> _pushTask;

        #endregion

        #region Constructors

        public TaskRunner(ITrackApplicationsService trackApplicationsService, ITrackHooksService trackHooksService, ISyncService syncService, ILogger<TaskRunner> logger)
        {
            _trackApplicationsService = trackApplicationsService ?? throw new ArgumentNullException(nameof(trackApplicationsService));
            _trackHooksService = trackHooksService ?? throw new ArgumentNullException(nameof(trackHooksService));
            _syncService = syncService ?? throw new ArgumentNullException(nameof(syncService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region ITaskRunner

        public Task Start()
        {
            //StartPushUpdates2API();
            return Task.FromResult(1);
        }

        public Task Stop()
        {
            //if (!_pushTask.IsCanceled)
            //    _pushTask.Dispose();
            return Task.FromResult(0);
        }

        #endregion

        #region Schedule methods

        private void StartPushUpdates2API() {
            var startDate = (DateTime?)null;
            var defaultSleepTimeMs = 30000; //todo: get from config
            var sleepTimeMs = defaultSleepTimeMs;
            do {
                _pushTask = _syncService.PushUpdatesAsync(startDate, null);
                var result = _pushTask.GetAwaiter().GetResult();
                if (result.Status)
                {
                    startDate = result.DataPushedUntil;
                    sleepTimeMs = defaultSleepTimeMs;
                }
                else {
                    sleepTimeMs *= 2; // increase intervals to avoid spamming API if it is unavailable or there is an error
                }
                System.Threading.Thread.Sleep(sleepTimeMs);
            } while (true);
        }

        #endregion
    }
}