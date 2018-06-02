using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTracker.Common;
using TimeTracker.Services.Storage;

namespace TimeTracker.Services.Tracking.Screenshots
{
    public interface IScreenshotService
    {
        Task<ResultBase> TrackAsync();
    }
}