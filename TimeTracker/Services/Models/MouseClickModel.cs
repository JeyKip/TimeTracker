using System.Windows.Forms;

namespace TimeTracker.Services.Models
{
    public class MouseClickModel
    {
        public MouseButtons Button { get; set; }
        public int Clicks { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}