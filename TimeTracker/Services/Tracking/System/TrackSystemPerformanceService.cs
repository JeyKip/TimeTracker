using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Threading.Tasks;
using TimeTracker.Common;
using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Tracking.System
{
    public interface ITrackSystemPerformanceService : ITakeSnapshot<SystemPerformanceSnapshot>
    {
        Task<ResultBase> Track();
    }

    public class TrackSystemPerformanceService : ITrackSystemPerformanceService
    {
        #region Fields and properties

        private readonly long _ramCapacity;
        private readonly ILogger<TrackSystemPerformanceService> _logger;
        private readonly PerformanceCounter _cpuUsageCounter;
        private readonly PerformanceCounter _ramUsageCounter;
        private readonly PerformanceCounter _uptimeCounter;
        private ConcurrentDictionary<Guid, SystemPerformanceSnapshotItem> _snapshotItems;

        #endregion

        #region Constructors

        public TrackSystemPerformanceService(ILogger<TrackSystemPerformanceService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _ramCapacity = GetRAMCapacity();
            _cpuUsageCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _ramUsageCounter = new PerformanceCounter("Memory", "Available MBytes");
            _uptimeCounter = new PerformanceCounter("System", "System Up Time");

            _snapshotItems = new ConcurrentDictionary<Guid, SystemPerformanceSnapshotItem>();

            // just to start counters, because it gets 0 at the first time
            _cpuUsageCounter.NextValue();
            _ramUsageCounter.NextValue();
            _uptimeCounter.NextValue();
        }

        #endregion

        #region ITrackSystemPerformanceService

        public bool ClearSnapshot(IEnumerable<Guid> idList)
        {
            if (idList == null)
                throw new ArgumentNullException(nameof(idList));

            var result = true;

            foreach (var snapshotItemId in idList)
            {
                if (!_snapshotItems.TryRemove(snapshotItemId, out SystemPerformanceSnapshotItem snapshotItem))
                {
                    _logger.LogError($"Cannot remove snapshot of system performance with id = {snapshotItemId}");
                    result = false;
                }
            }

            return result;
        }

        public SystemPerformanceSnapshot TakeSnapshot()
        {
            return new SystemPerformanceSnapshot
            {
                Items = _snapshotItems.Values
            };
        }

        public Task<ResultBase> Track()
        {
            return Task.Run(() =>
            {
                try
                {
                    var cpuUsage = (int)_cpuUsageCounter.NextValue();
                    var ramAvailableInBytes = (long)(_ramUsageCounter.NextValue() * 1024 * 1024);
                    var uptime = TimeSpan.FromSeconds(_uptimeCounter.NextValue());
                    var storageDevices = GetStorageDevices();

                    var snapshotItem = new SystemPerformanceSnapshotItem
                    {
                        Performance = new SystemPerformance
                        {
                            CpuUsage = cpuUsage,
                            Uptime = uptime,
                            Memory = new MemoryInfo
                            {
                                CapacityInBytes = _ramCapacity,
                                UsedInBytes = _ramCapacity - ramAvailableInBytes
                            },
                            Storages = storageDevices
                        }
                    };
                    if (!_snapshotItems.TryAdd(snapshotItem.Id, snapshotItem))
                    {
                        _logger.LogError("Failed to save snapshot of system performance.");
                        return new ResultBase { Status = false };
                    }

                    return new ResultBase { Status = true };
                }
                catch (Exception ex)
                {
                    return new ResultBase
                    {
                        Status = false,
                        ErrorMessage = ex.Message
                    };
                }
            });
        }

        #endregion

        #region Private Methods

        private long GetRAMCapacity()
        {
            long ramCapacity = 0;
            var query = "SELECT Capacity FROM Win32_PhysicalMemory";
            var searcher = new ManagementObjectSearcher(query);

            foreach (var module in searcher.Get())
                ramCapacity += Convert.ToInt64(module.Properties["Capacity"].Value);

            return ramCapacity;
        }

        private IEnumerable<StorageDevice> GetStorageDevices()
        {
            // get all logical hard drivers
            var query = "SELECT Name, FreeSpace, Size FROM Win32_LogicalDisk WHERE MediaType = '12'";
            var searcher = new ManagementObjectSearcher(query);

            foreach (var disk in searcher.Get())
            {
                yield return new StorageDevice
                {
                    Name = disk.Properties["Name"].Value.ToString(),
                    CapacityInBytes = Convert.ToInt64(disk.Properties["Size"].Value),
                    AvailableSpaceInBytes = Convert.ToInt64(disk.Properties["FreeSpace"].Value)
                };
            }
        }

        #endregion
    }
}