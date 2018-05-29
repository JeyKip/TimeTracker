using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTracker.Services.Storage
{
    public interface ITrackHooksStorageService
    {
        Task SaveMouseClick(MouseClick click);
        Task SaveKeyboardClick(char key);
    }
}
