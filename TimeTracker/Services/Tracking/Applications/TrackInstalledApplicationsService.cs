using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
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
        private List<InstalledApplicationsCheck> _checks = new List<InstalledApplicationsCheck>();
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        public TrackInstalledApplicationsService(ILogger<TrackInstalledApplicationsService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

                    var check = new InstalledApplicationsCheck();
                    check.Applications.AddRange(installed32Bits.Values);
                    check.Applications.AddRange(installed64Bits.Values.Where(x => !check.Applications.Any(s => s.Name == x.Name)));

                    lock (_lockObject)
                    {
                        _checks.Add(check);
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

        public Task Clear()
        {
            return Task.Run(() =>
            {
                lock (_lockObject)
                {
                    _checks.Clear();
                }
            });
        }

        #endregion

        #region ITakeSnapshot

        public InstalledApplicationsSnapshot TakeSnapshot()
        {
            lock (_lockObject)
            {
                var result = new InstalledApplicationsSnapshot();
                result.Checks.AddRange(_checks);
                Clear();
                return result;
            }
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