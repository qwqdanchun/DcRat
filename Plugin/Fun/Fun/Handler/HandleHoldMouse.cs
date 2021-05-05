using System;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace Plugin.Handler
{
    class HandleHoldMouse
    {
        public void Hold(string time)
        {
            var currentCursorPosition = Cursor.Position;
            var timer = new Timer { Interval = 5 };
            var currentTime = DateTime.UtcNow;
            timer.Elapsed += (sender, args) =>
            {
                Cursor.Position = currentCursorPosition;
                if ((DateTime.UtcNow - currentTime).TotalMilliseconds > int.Parse(time)*1000)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };
            timer.Start();
        }
    }
}
