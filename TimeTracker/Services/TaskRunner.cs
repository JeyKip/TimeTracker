using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeTracker.Common;
using TimeTracker.Services.Sync;
using TimeTracker.Services.Tracking;

namespace TimeTracker.Services
{
    public interface ITaskRunner
    {
        void Start();
        void Stop();
    }

    public class TaskRunner : ITaskRunner
    {
        #region Fields and properties

        private CancellationTokenSource _cancelTokenSource;
        private CancellationToken _cancellationToken;

        private readonly ITrackApplicationsService _trackApplicationsService;
        private readonly ITrackHooksService _trackHooksService;
        private readonly ISyncService _syncService;
        private readonly ILogger<TaskRunner> _logger;

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

        public void Start()
        {
            _cancelTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancelTokenSource.Token;

            ScheduleTask(async () => await _syncService.PushUpdatesAsync(null, null), 1000);
        }

        public void Stop()
        {
            _cancelTokenSource.Cancel();
            _cancelTokenSource.Dispose();
        }

        #endregion

        #region Schedule methods

        private void ScheduleTask(Func<Task<ResultBase>> action, int interval)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (interval <= 0)
                throw new ArgumentException("Interval must be a positive number", nameof(interval));

            var defaultSleepTimeMs = interval;
            var sleepTimeMs = defaultSleepTimeMs;

            Task.Run(async () =>
            {
                var sw = new Stopwatch();

                do
                {
                    sw.Start();

                    try
                    {
                        var result = await action();
                        if (result.Status)
                            sleepTimeMs = defaultSleepTimeMs;
                        else
                            throw new Exception(result.ErrorMessage);
                    }
                    catch (Exception ex)
                    {
                        sleepTimeMs *= 2;
                        _logger.LogError($"Error occurred during background method execution.", ex);
                    }

                    sw.Stop();

                    var elapsed = sw.ElapsedMilliseconds;
                    var sleepTime = (int)Math.Max(0, sleepTimeMs - elapsed);

                    sw.Reset();

                    Thread.Sleep(sleepTime);
                }
                while (!_cancellationToken.IsCancellationRequested);
            }, _cancellationToken);
        }

        #endregion
    }
}