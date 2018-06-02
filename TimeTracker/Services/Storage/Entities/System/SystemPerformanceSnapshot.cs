using System;
using System.Collections.Generic;

namespace TimeTracker.Services.Storage
{
    public class SystemPerformanceSnapshot
    {
        public IEnumerable<SystemPerformanceSnapshotItem> Items { get; set; }
    }

    public class SystemPerformanceSnapshotItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CheckDate { get; set; } = DateTime.UtcNow;
        public SystemPerformance Performance { get; set; }
    }
}