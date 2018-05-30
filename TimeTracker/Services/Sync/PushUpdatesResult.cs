using System;
using TimeTracker.Common;

namespace TimeTracker.Services.Sync
{
    public class PushUpdatesResult : ResultBase
    {
        public DateTime? DataPushedFrom { get; set; }
        public DateTime? DataPushedUntil { get; set; }
    }
}