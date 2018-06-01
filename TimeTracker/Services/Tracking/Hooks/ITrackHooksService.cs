namespace TimeTracker.Services.Tracking.Hooks
{
    public interface ITrackHooksService<THookModel>
        where THookModel : class
    {
        void TrackHook(THookModel entity);
        void Clear();
        void StartTracking();
        void StopTracking();
    }
}