﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Sync
{
    /// <summary>
    /// Provides ability to
    /// </summary>
    public class SyncService : ISyncService
    {
        private readonly ITrackHooksStorageService _trackHooksStorageService;
        private readonly ITrackApplicationsStorageService _trackApplicationsStorageService;
        private readonly ITrackApiWrapper _trackApiWrapper;
        private object _lockObject = new object();

        public SyncService(ITrackApplicationsStorageService trackApplicationsStorageService, ITrackHooksStorageService trackHooksStorageService, ITrackApiWrapper trackApiWrapper)
        {
            _trackHooksStorageService = trackHooksStorageService;
            _trackApplicationsStorageService = trackApplicationsStorageService;
            _trackApiWrapper = trackApiWrapper;
        }

        public DateTime LastSyncTime { get; private set; }

        /// <summary>
        /// Pushes user activity to API.
        /// </summary>
        /// <param name="startDate">Starting timestamp to filter data that already has been sent to API but not cleared yet.</param>
        /// <param name="endDate">???</param>
        /// <returns></returns>
        public async Task<PushUpdatesResult> PushUpdatesAsync(DateTime? startDate, DateTime? endDate)
        {
            var dtStart = startDate;
            var dtEnd = endDate ?? DateTime.UtcNow; //todo: remove endDate param
            var result = new PushUpdatesResult
            {
                Status = true,
            };

            // gather data from services
            var request = new PushUpdatesRequest
            {
                MouseClicks = await _trackHooksStorageService.GetMouseClicksAsync(dtStart, dtEnd),
                KeyboardClicks = await _trackHooksStorageService.GetKeyboardClicksAsync(dtStart, dtEnd)
            };

            // push request object to API
            var sendResult = await _trackApiWrapper.SendAsync(request);
            if (!sendResult.Status)
            {
                result.Status = false;
                result.ErrorMessage = "An error occurred while sending PushUpdatesRequest to API";

                return result;
            }

            result.DataPushedFrom = dtStart;
            result.DataPushedUntil = dtEnd;

            // clear items which were already posted to API
            var clearResult = await _trackHooksStorageService.ClearHooksAsync(dtEnd);
            if (!clearResult.Status)
            {
                // do nothing, data will be cleared in next iteration
                // while pushing of updates will not get already synced objects
            }

            LastSyncTime = DateTime.Now;

            return result;
        }
    }
}
