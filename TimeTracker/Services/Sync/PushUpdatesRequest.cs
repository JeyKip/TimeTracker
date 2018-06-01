using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Sync
{
    public class PushUpdatesRequest
    {
        public MouseClicksSnapshot MouseClicks { get; internal set; }
        public KeyboardClicksSnapshot KeyboardClicks { get; internal set; }
    }
}