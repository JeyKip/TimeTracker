using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Services.Models;
using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Tracking.Hooks
{
    public interface ITrackMouseClickService : ITrackHooksService<MouseClickModel>
    {
    }

    public class TrackMouseClickService : ITrackMouseClickService, ITakeSnapshot<MouseClicksSnapshot>
    {
        #region Fields and properties

        // maybe we should count separately left and right buttons clicks
        private int _mouseClicksCount = 0;
        private object _lockObject = new object();
        private readonly ILogger<TrackMouseClickService> _logger;
        private ConcurrentDictionary<Guid, MouseClicksSnapshot.MouseClicksSnapshotItem> _snapshotItems { get; set; }
        private DateTime? _timestamp = null;

        #endregion

        #region Constructors

        public TrackMouseClickService(ILogger<TrackMouseClickService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _snapshotItems = new ConcurrentDictionary<Guid, MouseClicksSnapshot.MouseClicksSnapshotItem>();
        }

        #endregion

        private bool IsTracking() {
            return _timestamp.HasValue;
        }

        public void Clear()
        {
            lock (_lockObject)
            {
                _mouseClicksCount = 0;
            }
        }

        public int GetHooksCount()
        {
            return _mouseClicksCount;
        }

        public void TrackHook(MouseClickModel entity)
        {
            // do not track clicks if tracking is not started
            if (!IsTracking())
                return;

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _logger.LogDebug($"TrackMouseHook: {Newtonsoft.Json.JsonConvert.SerializeObject(entity)}");
            lock (_lockObject)
            {
                _mouseClicksCount++;
            }
        }

        public MouseClicksSnapshot.MouseClicksSnapshotItem MakeSnapshot() {
            var now = DateTime.UtcNow;
            var count = 0;
            lock (_lockObject)
            {
                count = _mouseClicksCount;
                Clear();
            }
            var snapshotItem = new MouseClicksSnapshot.MouseClicksSnapshotItem
            {
                Id = Guid.NewGuid(),
                StartTime = _timestamp.Value,
                EndTime = now,
                MouseClickCount = count
            };
            if (IsTracking())
                _timestamp = now;
            if (!_snapshotItems.TryAdd(snapshotItem.Id, snapshotItem))
            {
                _logger.LogError("Failed to make a snapshot of mouse clicks");
                return null;
            }
            // update timestamp to be able calculate duration of next tracking record
            return snapshotItem;
        }

        public MouseClicksSnapshot TakeSnapshot()
        {
            if (IsTracking())
            {
                MakeSnapshot();
            }
            var result = new MouseClicksSnapshot
            {
                Items = _snapshotItems.Values.AsEnumerable()
            };
            return result;
        }

        public bool ClearSnapshot(IEnumerable<Guid> idList)
        {
            var result = true;
            foreach (var id in idList)
            {
                if (!_snapshotItems.TryRemove(id, out var removedItem))
                    _logger.LogError($"Failed to remove mouse snapshot with Id {id}");
            }
            return result;
        }

        public void StartTracking()
        {
            _timestamp = DateTime.UtcNow;
        }

        public void StopTracking()
        {
            MakeSnapshot();
            _timestamp = null;
        }
    }
}