using System;
using System.Collections.Generic;

namespace TimeTracker.Services.Storage
{
    public class DnsCacheSnapshot
    {
        public IEnumerable<DnsCacheSnapshotItem> Items { get; set; }
    }

    public class DnsCacheSnapshotItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CheckDate { get; set; } = DateTime.UtcNow;
        public DnsCacheInfo CacheInfo { get; set; }
    }
}