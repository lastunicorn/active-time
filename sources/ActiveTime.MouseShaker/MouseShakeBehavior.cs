using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace DustInTheWind.ActiveTime.MouseShaker
{
    internal class MouseShakeBehavior
    {
        private const double OneSecond = 1000;
        private const double OneMinute = OneSecond * 60;

        private readonly Timer timer;
        private readonly Random random;

        public MouseShakeBehavior()
        {
            random = new Random();

            timer = new Timer(OneMinute);
            timer.Elapsed += HandleTimerElapsed;
        }

        private void HandleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    MoveCursor();
                    Thread.Sleep(50);
                }
            });
        }

        private void MoveCursor()
        {
            const int delta = 10;

            Point mousePosition = Cursor.Position;
            mousePosition.X += random.Next(-delta, delta + 1);
            mousePosition.Y += random.Next(-delta, delta + 1);

            Cursor.Position = mousePosition;
        }

        public void Start()
        {
            UserActivityMonitor.HookManager.MouseMove += HandleMouseMoved;
            timer.Start();
        }

        public void Stop()
        {
            UserActivityMonitor.HookManager.MouseMove -= HandleMouseMoved;
            timer.Stop();
        }

        private void HandleMouseMoved(object sender, EventArgs e)
        {
            timer.Stop();
            timer.Start();
        }
    }
}