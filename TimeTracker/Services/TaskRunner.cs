using Keystroke.API;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeTracker.Common;
using TimeTracker.Services.Models;
using TimeTracker.Services.Sync;
using TimeTracker.Services.Tracking;
using TimeTracker.Services.Tracking.Applications;
using TimeTracker.Services.Tracking.Hooks;
using TimeTracker.Services.Tracking.System;
using TimeTracker.Services.Tracking.Screenshots;
using TimeTracker.Properties;

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

        private readonly ITrackInstalledApplicationsService _trackInstalledApplicationsService;
        private readonly ITrackOpenedApplicationsService _trackOpenedApplicationsService;
        private readonly IScreenshotService _screenshotService;
        private readonly ITrackKeystrokeService _trackKeystrokeService;
        private readonly ITrackMouseClickService _trackMouseClickService;
        private readonly ITrackDnsCacheService _trackDnsCacheService;
        private readonly ITrackSystemPerformanceService _trackSystemPerformanceService;
        private readonly ISyncService _syncService;
        private readonly KeystrokeAPI _keystrokeAPI;
        private readonly ILogger<TaskRunner> _logger;

        #endregion

        #region Constructors

        public TaskRunner(
            ITrackInstalledApplicationsService trackInstalledApplicationsService,
            ITrackOpenedApplicationsService trackOpenedApplicationsService,
            ITrackKeystrokeService trackKeystrokeService,
            ITrackMouseClickService trackMouseClickService,
            ITrackDnsCacheService trackDnsCacheService,
            IScreenshotService screenshotService,
            ITrackSystemPerformanceService trackSystemPerformanceService,
            ISyncService syncService,
            KeystrokeAPI keystrokeAPI,
            ILogger<TaskRunner> logger)
        {
            _trackInstalledApplicationsService = trackInstalledApplicationsService ?? throw new ArgumentNullException(nameof(trackInstalledApplicationsService));
            _trackOpenedApplicationsService = trackOpenedApplicationsService ?? throw new ArgumentNullException(nameof(trackOpenedApplicationsService));
            _trackKeystrokeService = trackKeystrokeService ?? throw new ArgumentNullException(nameof(trackKeystrokeService));
            _trackMouseClickService = trackMouseClickService ?? throw new ArgumentNullException(nameof(trackMouseClickService));
            _trackDnsCacheService = trackDnsCacheService ?? throw new ArgumentNullException(nameof(trackDnsCacheService));
            _trackSystemPerformanceService = trackSystemPerformanceService ?? throw new ArgumentNullException(nameof(trackSystemPerformanceService));
            _syncService = syncService ?? throw new ArgumentNullException(nameof(syncService));
            _keystrokeAPI = keystrokeAPI ?? throw new ArgumentNullException(nameof(keystrokeAPI));
            _screenshotService = screenshotService ?? throw new ArgumentNullException(nameof(screenshotService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region ITaskRunner

        public void Start()
        {
            _cancelTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancelTokenSource.Token;

            ScheduleRecurringTask(async () => await _trackInstalledApplicationsService.TrackApplications(), Settings.Default.TrackInstalledApplicationsInterval);
            ScheduleRecurringTask(async () => await _trackOpenedApplicationsService.TrackApplications(), Settings.Default.TrackOpenedApplicationsInterval);
            ScheduleRecurringTask(async () => await _trackDnsCacheService.Track(), Settings.Default.TrackDnsCacheInterval);
            ScheduleRecurringTask(async () => await _screenshotService.TrackAsync(), Settings.Default.TrackScreenshotsInterval);
            ScheduleRecurringTask(async () => await _trackSystemPerformanceService.Track(), Settings.Default.TrackSystemPerformanceInterval);
            ScheduleRecurringTask(async () => await _syncService.PushUpdatesAsync(), Settings.Default.SyncInterval);
            ScheduleKeystrokeTask();
            ScheduleMouseClickTask();
        }

        public void Stop()
        {
            _cancelTokenSource.Cancel();
            _cancelTokenSource.Dispose();
            _keystrokeAPI.RemoveKeyboardHook();
            _keystrokeAPI.RemoveMouseHook();
            _trackKeystrokeService.StopTracking();
            _trackMouseClickService.StopTracking();
        }

        #endregion

        #region Schedule methods

        private void ScheduleRecurringTask(Func<Task<ResultBase>> action, int interval)
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

                // we do not need to track information instantly so we need delay before first run
                Thread.Sleep(sleepTimeMs);

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
                        _logger.LogError($"Error occurred during background method execution: {ex.Message}.", ex);
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

        private void ScheduleKeystrokeTask()
        {
            _trackKeystrokeService.StartTracking();
            _keystrokeAPI.CreateKeyboardHook((key) =>
            {
                _trackKeystrokeService.TrackHook(new KeystrokeModel
                {
                    KeyCode = (int)key.KeyCode
                });
            });
        }

        private void ScheduleMouseClickTask()
        {
            _trackMouseClickService.StartTracking();
            _keystrokeAPI.CreateMouseHook((mouse) =>
            {
                _trackMouseClickService.TrackHook(new MouseClickModel
                {
                    Button = mouse.ButtonCode,
                    X = mouse.X,
                    Y = mouse.Y
                });
            });
        }

        #endregion
    }
}