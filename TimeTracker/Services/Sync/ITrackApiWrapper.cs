using System.Threading.Tasks;

namespace TimeTracker.Services.Sync
{
    public interface ITrackApiWrapper
    {
        Task<SendAsyncResult> SendAsync(PushUpdatesRequest request);
    }
}