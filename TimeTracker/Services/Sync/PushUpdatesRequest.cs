using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Sync
{
    public class PushUpdatesRequest
    {
        public object MouseClicks { get; internal set; }
        public KeyboardClicksSnapshot KeyboardClicks { get; internal set; }
    }
}