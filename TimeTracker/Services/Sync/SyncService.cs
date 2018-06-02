using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Services.Storage;
using TimeTracker.Services.Tracking;

namespace TimeTracker.Services.Sync
{
    /// <summary>
    /// Provides ability to
    /// </summary>
    public class SyncService : ISyncService
    {
        private readonly ITrackApiWrapper _trackApiWrapper;
        private readonly ITakeSnapshot<MouseClicksSnapshot> _mouseSnapshot;
        private readonly ITakeSnapshot<KeyboardClicksSnapshot> _keyboardSnapshot;
        private readonly ITakeSnapshot<InstalledApplicationsSnapshot> _installedApplicationsSnapshot;
        private readonly ITakeSnapshot<OpenedApplicationsSnapshot> _openedApplicationsSnapshot;
        private readonly ITakeSnapshot<DnsCacheSnapshot> _dnsCacheSnapshot;
        private readonly ITakeSnapshot<SystemPerformanceSnapshot> _systemPerformanceSnapshot;

        public SyncService(
            ITrackApiWrapper trackApiWrapper,
            ITakeSnapshot<MouseClicksSnapshot> mouseSnapshot,
            ITakeSnapshot<KeyboardClicksSnapshot> keyboardSnapshot,
            ITakeSnapshot<InstalledApplicationsSnapshot> installedApplicationsSnapshot,
            ITakeSnapshot<OpenedApplicationsSnapshot> openedApplicationsSnapshot,
            ITakeSnapshot<DnsCacheSnapshot> dnsCacheSnapshot,
            ITakeSnapshot<SystemPerformanceSnapshot> systemPerformanceSnapshot)
        {
            _trackApiWrapper = trackApiWrapper;
            _mouseSnapshot = mouseSnapshot;
            _keyboardSnapshot = keyboardSnapshot;
            _installedApplicationsSnapshot = installedApplicationsSnapshot;
            _openedApplicationsSnapshot = openedApplicationsSnapshot;
            _dnsCacheSnapshot = dnsCacheSnapshot;
            _systemPerformanceSnapshot = systemPerformanceSnapshot;
        }

        public DateTime LastSyncTime { get; private set; }

        /// <summary>
        /// Pushes user activity to API.
        /// </summary>
        /// <param name="startDate">Starting timestamp to filter data that already has been sent to API but not cleared yet.</param>
        /// <param name="endDate">???</param>
        /// <returns></returns>
        public async Task<PushUpdatesResult> PushUpdatesAsync()
        {
            var result = new PushUpdatesResult
            {
                Status = true,
            };

            // gather data from services
            var request = new PushUpdatesRequest
            {
                MouseClicks = _mouseSnapshot.TakeSnapshot(),
                KeyboardClicks = _keyboardSnapshot.TakeSnapshot(),
                InstalledApplications = _installedApplicationsSnapshot.TakeSnapshot(),
                OpenedApplications = _openedApplicationsSnapshot.TakeSnapshot(),
                DnsCache = _dnsCacheSnapshot.TakeSnapshot(),
                SystemPerformance = _systemPerformanceSnapshot.TakeSnapshot()
            };

            // push request object to API
            var sendResult = await _trackApiWrapper.SendAsync(request);
            if (!sendResult.Status)
            {
                result.Status = false;
                result.ErrorMessage = "An error occurred while sending PushUpdatesRequest to API";

                return result;
            }

            // gather ids of handles snapshot items
            result.MouseIdList = request.MouseClicks.Items.Select(t => t.Id);
            result.KeyboardIdList = request.KeyboardClicks.Items.Select(t => t.Id);
            result.InstalledAppsIdList = request.InstalledApplications.Items.Select(t => t.Id);
            result.OpenedAppsIdList = request.OpenedApplications.Items.Select(t => t.Id);
            result.DnsCacheIdList = request.DnsCache.Items.Select(t => t.Id);
            result.SystemPerformanceIdList = request.SystemPerformance.Items.Select(t => t.Id);

            // clear items which were already posted to API
            _mouseSnapshot.ClearSnapshot(result.MouseIdList);
            _keyboardSnapshot.ClearSnapshot(result.KeyboardIdList);
            _installedApplicationsSnapshot.ClearSnapshot(result.InstalledAppsIdList);
            _openedApplicationsSnapshot.ClearSnapshot(result.OpenedAppsIdList);
            _dnsCacheSnapshot.ClearSnapshot(result.DnsCacheIdList);
            _systemPerformanceSnapshot.ClearSnapshot(result.SystemPerformanceIdList);

            LastSyncTime = DateTime.UtcNow;

            return result;
        }
    }
}