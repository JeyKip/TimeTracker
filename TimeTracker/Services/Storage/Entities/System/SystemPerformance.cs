using System;
using System.Collections.Generic;

namespace TimeTracker.Services.Storage
{
    public class SystemPerformance
    {
        public int CpuUsage { get; set; }
        public TimeSpan Uptime { get; set; }
        public MemoryInfo Memory { get; set; }
        public IEnumerable<StorageDevice> Storages { get; set; }
    }

    public class MemoryInfo
    {
        public long UsedInBytes { get; set; }
        public long UsedInPercents { get { return (long)((UsedInBytes / (float)CapacityInBytes) * 100); } }
        public long CapacityInBytes { get; set; }
    }

    public class StorageDevice
    {
        public string Name { get; set; }
        public long CapacityInBytes { get; set; }
        public long AvailableSpaceInBytes { get; set; }
    }
}