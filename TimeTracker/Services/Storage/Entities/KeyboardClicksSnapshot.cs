using System;
using System.Collections.Generic;

namespace TimeTracker.Services.Storage
{
    public class KeyboardClicksSnapshot
    {
        public class KeyboardClickSnapshotItem
        {
            public Guid Id { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public int PressButtonCount { get; set; }
        }

        public IEnumerable<KeyboardClickSnapshotItem> Items { get; set; }
    }
}