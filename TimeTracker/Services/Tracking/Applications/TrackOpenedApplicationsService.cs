using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Common;
using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Tracking.Applications
{
    public interface ITrackOpenedApplicationsService : ITrackApplicationsService, ITakeSnapshot<OpenedApplicationsSnapshot>
    {

    }

    public class TrackOpenedApplicationsService : ITrackOpenedApplicationsService
    {
        #region Fields and properties

        private readonly ILogger<TrackOpenedApplicationsService> _logger;
        private ConcurrentDictionary<Guid, OpenedApplicationsSnapshotItem> _snapshotItems;

        #endregion

        #region Constructors

        public TrackOpenedApplicationsService(ILogger<TrackOpenedApplicationsService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _snapshotItems = new ConcurrentDictionary<Guid, OpenedApplicationsSnapshotItem>();
        }

        #endregion

        #region ITrackApplicationsService

        public Task<ResultBase> TrackApplications()
        {
            return Task.Run(() =>
            {
                try
                {
                    var applications = GetApplications();
                    var check = new OpenedApplicationsSnapshotItem();
                    check.Applications.AddRange(applications);

                    if (!_snapshotItems.TryAdd(check.Id, check))
                    {
                        _logger.LogError("Failed to save snapshot of opened applications.");
                        return new ResultBase { Status = false };
                    }

                    return new ResultBase { Status = true };
                }
                catch (Exception ex)
                {
                    return new ResultBase
                    {
                        Status = false,
                        ErrorMessage = ex.Message
                    };
                }
            });
        }

        #endregion

        #region ITakeSnapshot

        public OpenedApplicationsSnapshot TakeSnapshot()
        {
            return new OpenedApplicationsSnapshot
            {
                Items = _snapshotItems.Values
            };
        }

        public bool ClearSnapshot(IEnumerable<Guid> idList)
        {
            if (idList == null)
                throw new ArgumentNullException(nameof(idList));

            var result = true;

            foreach (var checkId in idList)
            {
                if (!_snapshotItems.TryRemove(checkId, out OpenedApplicationsSnapshotItem check))
                {
                    _logger.LogError($"Cannot remove check of installed applications with id = {checkId}");
                    result = false;
                }
            }

            return result;
        }

        #endregion

        #region Private Methods

        private IEnumerable<OpenedApplication> GetApplications()
        {
            var applications = new Dictionary<string, OpenedApplication>();
            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                string name = null;
                try
                {
                    var module = process.MainModule;
                    if (module != null)
                    {
                        name = module.FileVersionInfo.FileDescription;
                        if (string.IsNullOrWhiteSpace(name))
                            name = module.FileVersionInfo.ProductName;
                    }
                }
                catch
                {
                    // do nothing
                }
                finally
                {
                    if (string.IsNullOrWhiteSpace(name))
                        name = process.ProcessName;
                }
                if (applications.ContainsKey(name))
                    continue;

                applications.Add(name, new OpenedApplication
                {
                    Name = name
                });
            }

            return applications.Values;
        }

        #endregion
    }
}