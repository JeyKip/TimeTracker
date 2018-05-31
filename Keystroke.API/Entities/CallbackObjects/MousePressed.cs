namespace Keystroke.API.CallbackObjects
{
    public class MousePressed
    {
        public MouseButtonCode ButtonCode { get; set; }
        public string CurrentWindow { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}