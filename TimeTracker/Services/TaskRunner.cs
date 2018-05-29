using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Services.Tracking;

namespace TimeTracker.Services
{
    public interface ITaskRunner
    {
        Task Start();
        Task Stop();
    }

    public class TaskRunner : ITaskRunner
    {
        #region Fields and properties

        private readonly ITrackApplicationsService _trackApplicationsService;
        private readonly ITrackHooksService _trackHooksService;
        private readonly ILogger<TaskRunner> _logger;

        #endregion

        #region Constructors

        public TaskRunner(ITrackApplicationsService trackApplicationsService, ITrackHooksService trackHooksService, ILogger<TaskRunner> logger)
        {
            _trackApplicationsService = trackApplicationsService ?? throw new ArgumentNullException(nameof(trackApplicationsService));
            _trackHooksService = trackHooksService ?? throw new ArgumentNullException(nameof(trackHooksService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region ITaskRunner

        public Task Start()
        {
            return Task.FromResult(1);
        }

        public Task Stop()
        {
            return Task.FromResult(0);
        }

        #endregion
    }
}