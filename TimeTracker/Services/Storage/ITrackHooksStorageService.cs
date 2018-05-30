using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTracker.Common;

namespace TimeTracker.Services.Storage
{
    public interface ITrackHooksStorageService
    {
        Task SaveMouseClick(MouseClick click);
        Task SaveKeyboardClick(char key);
        Task<ResultBase> ClearHooksAsync(DateTime EndDate);
        Task<MouseClicksSnapshot> GetMouseClicksAsync(DateTime? dtStart, DateTime dtEnd);
        Task<KeyboardClicksSnapshot> GetKeyboardClicksAsync(DateTime? dtStart, DateTime dtEnd);
    }
}
