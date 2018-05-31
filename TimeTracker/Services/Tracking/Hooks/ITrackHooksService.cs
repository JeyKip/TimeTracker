namespace TimeTracker.Services.Tracking.Hooks
{
    public interface ITrackHooksService<T>
        where T : class
    {
        void TrackHook(T entity);
        int GetHooksCount();
        void Clear();
    }
}