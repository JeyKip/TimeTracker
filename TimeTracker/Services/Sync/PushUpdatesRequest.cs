using System;
using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Sync
{
    public class PushUpdatesRequest
    {
        public MouseClicksSnapshot MouseClicks { get; internal set; }
        public KeyboardClicksSnapshot KeyboardClicks { get; internal set; }
        public InstalledApplicationsSnapshot InstalledApplications { get; internal set; }
        public OpenedApplicationsSnapshot OpenedApplications { get; internal set; }
        public DnsCacheSnapshot DnsCache { get; internal set; }
        public ScreenshotSnapshot Screenshots { get; internal set; }
        public SystemPerformanceSnapshot SystemPerformance { get; internal set; }
        public TimeSpan UserTimeZoneOffset { get; internal set; }
    }
}