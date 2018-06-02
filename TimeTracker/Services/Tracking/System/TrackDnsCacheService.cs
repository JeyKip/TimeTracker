using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TimeTracker.Common;
using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Tracking.System
{
    public interface ITrackDnsCacheService : ITakeSnapshot<DnsCacheSnapshot>
    {
        Task<ResultBase> Track();
    }

    public class TrackDnsCacheService : ITrackDnsCacheService
    {
        #region Fields and properties

        private readonly ILogger<TrackDnsCacheService> _logger;
        private ConcurrentDictionary<Guid, DnsCacheSnapshotItem> _snapshotItems;

        #endregion

        #region Constructors

        public TrackDnsCacheService(ILogger<TrackDnsCacheService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _snapshotItems = new ConcurrentDictionary<Guid, DnsCacheSnapshotItem>();
        }

        #endregion

        #region ITrackDnsCacheService

        public bool ClearSnapshot(IEnumerable<Guid> idList)
        {
            if (idList == null)
                throw new ArgumentNullException(nameof(idList));

            var result = true;

            foreach (var snapshotItemId in idList)
            {
                if (!_snapshotItems.TryRemove(snapshotItemId, out DnsCacheSnapshotItem snapshotItem))
                {
                    _logger.LogError($"Cannot remove snapshot of DNS Cache with id = {snapshotItemId}");
                    result = false;
                }
            }

            return result;
        }

        public DnsCacheSnapshot TakeSnapshot()
        {
            return new DnsCacheSnapshot
            {
                Items = _snapshotItems.Values
            };
        }

        public Task<ResultBase> Track()
        {
            return Task.Run(() =>
            {
                try
                {
                    var dnsInformation = ReadDNSCacheInformation();
                    var snapshotItem = new DnsCacheSnapshotItem
                    {
                        CacheInfo = new DnsCacheInfo
                        {
                            RawDnsInformation = dnsInformation
                        }
                    };

                    if (!_snapshotItems.TryAdd(snapshotItem.Id, snapshotItem))
                    {
                        _logger.LogError("Failed to save snapshot of DNS Cache Information.");
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

        #region Private Methods

        private string ReadDNSCacheInformation()
        {
            try
            {
                // Create the ProcessStartInfo using "cmd" as the program to be run, and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows, and then exit.
                // Command "chcp 437" sets English language for output
                // Command "ipconfig /displaydns" displays all records from DNS Cache
                var procStartInfo = new ProcessStartInfo("cmd", "/c \"chcp 437 && ipconfig /displaydns\"")
                {
                    // The following commands are needed to redirect the standard output.
                    // This means that it will be redirected to the Process.StandardOutput StreamReader.
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    // Do not create the black window.
                    CreateNoWindow = true,
                };
                // Now we create a process, assign its ProcessStartInfo and start it
                var proccess = new Process
                {
                    StartInfo = procStartInfo
                };
                proccess.Start();
                // Get the output into a string
                var result = proccess.StandardOutput.ReadToEnd();
                // Cut first row about setting current language code
                if (result != null)
                    result = result.Substring(result.IndexOf("Windows IP Configuration"));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot read DNS cache information from cmd.");

                return null;
            }
        }

        #endregion
    }
}