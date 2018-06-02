using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Services.Storage;
using TimeTracker.Services.Tracking;
using TimeTracker.Services.Tracking.Screenshots;

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
        private readonly ITakeSnapshot<ScreenshotSnapshot> _screenshotService;
        private object _lockObject = new object();

        public SyncService(ITrackApiWrapper trackApiWrapper, ITakeSnapshot<MouseClicksSnapshot> mouseSnapshot, ITakeSnapshot<KeyboardClicksSnapshot> keyboardSnapshot,
            ITakeSnapshot<ScreenshotSnapshot> screenshotService)
        {
            _trackApiWrapper = trackApiWrapper;
            _mouseSnapshot = mouseSnapshot;
            _keyboardSnapshot = keyboardSnapshot;
            _screenshotService = screenshotService;
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
                Screenshots = _screenshotService.TakeSnapshot()
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
            result.MouseIdList = request.MouseClicks.Items.Select(t=>t.Id);
            result.KeyboardIdList = request.KeyboardClicks.Items.Select(t => t.Id);

            // clear items which were already posted to API
            _mouseSnapshot.ClearSnapshot(result.MouseIdList);
            _keyboardSnapshot.ClearSnapshot(result.KeyboardIdList);
            _screenshotService.ClearSnapshot(request.Screenshots.Screenshots.Select(t=>t.Id));

            LastSyncTime = DateTime.Now;

            return result;
        }
    }
}
