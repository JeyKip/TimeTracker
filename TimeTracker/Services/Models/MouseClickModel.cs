using Keystroke.API;

namespace TimeTracker.Services.Models
{
    public class MouseClickModel
    {
        public MouseButtonCode Button { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}