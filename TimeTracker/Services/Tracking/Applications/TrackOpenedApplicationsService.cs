using Microsoft.Extensions.Logging;
using System;
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
        private List<OpenedApplicationsCheck> _checks = new List<OpenedApplicationsCheck>();
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        public TrackOpenedApplicationsService(ILogger<TrackOpenedApplicationsService> logger)
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
                    var applications = GetApplications();
                    var check = new OpenedApplicationsCheck();
                    check.Applications.AddRange(applications);

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

        public OpenedApplicationsSnapshot TakeSnapshot()
        {
            lock (_lockObject)
            {
                var result = new OpenedApplicationsSnapshot();
                result.Checks.AddRange(_checks);
                Clear();
                return result;
            }
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