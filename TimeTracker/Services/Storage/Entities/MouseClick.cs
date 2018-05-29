using System.Windows.Forms;

namespace TimeTracker.Services.Storage
{
    public class MouseClick
    {
        public MouseButtons Button { get; set; }
        public int Clicks { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}