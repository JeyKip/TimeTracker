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
    public interface ITrackKeystrokeService : ITrackHooksService<KeystrokeModel>
    {
    }

    public class TrackKeystrokeService : ITrackKeystrokeService, ITakeSnapshot<KeyboardClicksSnapshot>
    {
        #region Fields and properties

        private int _keystrokesCount = 0;
        private object _lockObject = new object();
        private readonly ILogger<TrackKeystrokeService> _logger;
        private ConcurrentDictionary<Guid, KeyboardClicksSnapshot.KeyboardClickSnapshotItem> _snapshotItems { get; set; }
        private DateTime? _timestamp = null;

        #endregion

        #region Constructors

        public TrackKeystrokeService(ILogger<TrackKeystrokeService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _snapshotItems = new ConcurrentDictionary<Guid, KeyboardClicksSnapshot.KeyboardClickSnapshotItem>();
        }

        #endregion

        private bool IsTracking()
        {
            return _timestamp.HasValue;
        }

        public void Clear()
        {
            lock (_lockObject)
            {
                _keystrokesCount = 0;
            }
        }

        public int GetHooksCount()
        {
            return _keystrokesCount;
        }

        public void TrackHook(KeystrokeModel entity)
        {
            // do not track clicks if tracking is not started
            if (!IsTracking())
                return;

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _logger.LogDebug($"TrackHook: {entity.KeyCode}");
            lock (_lockObject) {
                _keystrokesCount++;
            }
        }

        public KeyboardClicksSnapshot.KeyboardClickSnapshotItem MakeSnapshot()
        {
            var now = DateTime.UtcNow;
            var count = 0;
            lock (_lockObject)
            {
                count = _keystrokesCount;
                Clear();
            }
            var snapshotItem = new KeyboardClicksSnapshot.KeyboardClickSnapshotItem
            {
                Id = Guid.NewGuid(),
                StartTime = _timestamp.Value,
                EndTime = now,
                PressButtonCount = count
            };
            if (IsTracking())
                _timestamp = now;
            if (!_snapshotItems.TryAdd(snapshotItem.Id, snapshotItem))
            {
                _logger.LogError("Failed to make a snapshot of keyboard clicks");
                return null;
            }
            // update timestamp to be able calculate duration of next tracking record
            return snapshotItem;
        }

        public KeyboardClicksSnapshot TakeSnapshot()
        {
            if (IsTracking())
            {
                MakeSnapshot();
            }
            var result = new KeyboardClicksSnapshot
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
                    _logger.LogError($"Failed to remove keyboard snapshot with Id {id}");
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