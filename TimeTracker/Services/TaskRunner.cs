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
using TimeTracker.Services.Tracking.Hooks;

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
        private readonly ITrackKeystrokeService _trackKeystrokeService;
        private readonly ISyncService _syncService;
        private readonly KeystrokeAPI _keystrokeAPI;
        private readonly ILogger<TaskRunner> _logger;

        #endregion

        #region Constructors

        public TaskRunner(ITrackApplicationsService trackApplicationsService, ITrackKeystrokeService trackKeystrokeService, ISyncService syncService, KeystrokeAPI keystrokeAPI, ILogger<TaskRunner> logger)
        {
            _trackApplicationsService = trackApplicationsService ?? throw new ArgumentNullException(nameof(trackApplicationsService));
            _trackKeystrokeService = trackKeystrokeService ?? throw new ArgumentNullException(nameof(trackKeystrokeService));
            _syncService = syncService ?? throw new ArgumentNullException(nameof(syncService));
            _keystrokeAPI = keystrokeAPI ?? throw new ArgumentNullException(nameof(keystrokeAPI));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region ITaskRunner

        public void Start()
        {
            _cancelTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancelTokenSource.Token;

            ScheduleRecurringTask(async () => await _syncService.PushUpdatesAsync(null, null), 1000);
            ScheduleKeystrokeTask();
        }

        public void Stop()
        {
            _cancelTokenSource.Cancel();
            _cancelTokenSource.Dispose();
            _keystrokeAPI.RemoveKeyboardHook();
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

        private void ScheduleKeystrokeTask()
        {
            _keystrokeAPI.CreateKeyboardHook((key) =>
            {
                _trackKeystrokeService.TrackHook(new KeystrokeModel
                {
                    KeyCode = (int)key.KeyCode
                });
            });
        }

        #endregion
    }
}