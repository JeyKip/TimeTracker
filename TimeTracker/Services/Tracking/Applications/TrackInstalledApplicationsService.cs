using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.Common;
using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Tracking.Applications
{
    public interface ITrackInstalledApplicationsService : ITrackApplicationsService, ITakeSnapshot<InstalledApplicationsSnapshot>
    {

    }

    public class TrackInstalledApplicationsService : ITrackInstalledApplicationsService
    {
        #region Fields and properties

        private readonly ILogger<TrackInstalledApplicationsService> _logger;
        private ConcurrentDictionary<Guid, InstalledApplicationsSnapshotItem> _snapshotItems;

        #endregion

        #region Constructors

        public TrackInstalledApplicationsService(ILogger<TrackInstalledApplicationsService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _snapshotItems = new ConcurrentDictionary<Guid, InstalledApplicationsSnapshotItem>();
        }

        #endregion

        #region ITrackApplicationsService

        public Task<ResultBase> TrackApplications()
        {
            return Task.Run(() =>
            {
                try
                {
                    var installed32Bits = GetApplications(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
                    var installed64Bits = GetApplications(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall");

                    var check = new InstalledApplicationsSnapshotItem();
                    check.Applications.AddRange(installed32Bits.Values);
                    check.Applications.AddRange(installed64Bits.Values.Where(x => !check.Applications.Any(s => s.Name == x.Name)));

                    if (!_snapshotItems.TryAdd(check.Id, check))
                    {
                        _logger.LogError("Failed to save snapshot of installed applications.");
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

        public InstalledApplicationsSnapshot TakeSnapshot()
        {
            return new InstalledApplicationsSnapshot
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
                if (!_snapshotItems.TryRemove(checkId, out InstalledApplicationsSnapshotItem check))
                {
                    _logger.LogError($"Cannot remove check of installed applications with id = {checkId}");
                    result = false;
                }
            }

            return result;
        }

        #endregion

        #region Private Methods

        private Dictionary<string, InstalledApplication> GetApplications(string registryKey)
        {
            var applications = new Dictionary<string, InstalledApplication>();

            using (var key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                foreach (var subkeyName in key.GetSubKeyNames())
                {
                    using (var subkey = key.OpenSubKey(subkeyName))
                    {
                        var name = (subkey.GetValue("DisplayName") ?? "").ToString();
                        if (string.IsNullOrWhiteSpace(name) || applications.ContainsKey(name))
                            continue;

                        var application = new InstalledApplication
                        {
                            Name = name.ToString()
                        };
                        applications.Add(name, application);
                    }
                }
            }

            return applications;
        }

        #endregion
    }
}