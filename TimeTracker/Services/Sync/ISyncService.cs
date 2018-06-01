using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.Services.Sync
{
    public interface ISyncService
    {
        Task<PushUpdatesResult> PushUpdatesAsync();
    }
}
