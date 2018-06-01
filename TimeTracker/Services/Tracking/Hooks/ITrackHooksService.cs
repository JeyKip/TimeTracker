using System;
using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Tracking.Hooks
{
    public interface ITrackHooksService<THookModel>
        where THookModel : class
    {
        void TrackHook(THookModel entity);
        int GetHooksCount();
        void Clear();
        void StartTracking();
        void StopTracking();
    }
}