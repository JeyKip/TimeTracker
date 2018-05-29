using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Services.Storage
{
    public class TrackStorageService : ITrackApplicationsStorageService, ITrackHooksStorageService
    {
        #region ITrackApplicationsStorageService

        public Task SaveInstalledApplications(IEnumerable<TrackApplication> applications)
        {
            throw new NotImplementedException();
        }

        public Task SaveOpenedApplications(IEnumerable<TrackApplication> applications)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITrackHooksStorageService

        public Task SaveKeyboardClick(char key)
        {
            throw new NotImplementedException();
        }

        public Task SaveMouseClick(MouseClick click)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}