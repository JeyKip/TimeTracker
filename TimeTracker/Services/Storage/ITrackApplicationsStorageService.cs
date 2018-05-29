using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeTracker.Services.Storage
{
    public interface ITrackApplicationsStorageService
    {
        Task SaveInstalledApplications(IEnumerable<TrackApplication> applications);
        Task SaveOpenedApplications(IEnumerable<TrackApplication> applications);
    }
}