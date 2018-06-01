using System;
using System.Collections.Generic;

namespace TimeTracker.Services.Storage
{
    public class MouseClicksSnapshot
    {
        public class MouseClicksSnapshotItem {
            public Guid Id { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public int MouseClickCount { get; set; }
        }

        public IEnumerable<MouseClicksSnapshotItem> Items { get; set; }
    }
}