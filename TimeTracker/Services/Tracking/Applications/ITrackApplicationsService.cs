using System.Threading.Tasks;
using TimeTracker.Common;

namespace TimeTracker.Services.Tracking.Applications
{
    public interface ITrackApplicationsService
    {
        Task<ResultBase> TrackApplications();
        Task Clear();
    }
}