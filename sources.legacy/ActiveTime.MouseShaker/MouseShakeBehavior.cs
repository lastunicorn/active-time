using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using DustInTheWind.ActiveTime.MouseShaker.WindowsApi;
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
                for (int i = 0; i < 100; i++)
                {
                    MoveCursor();
                    Thread.Sleep(20);
                }
            });
        }

        private void MoveCursor()
        {
            const int delta = 30;

            User32Library.GetCursorPos(out Point mousePosition);

            int newX = mousePosition.X + random.Next(-delta, delta + 1);
            int newY = mousePosition.Y + random.Next(-delta, delta + 1);

            User32Library.SetCursorPos(newX, newY);
        }

        public void Start()
        {
            UserActivityMonitor.UserInputActivity.MouseMove += HandleMouseMoved;
            timer.Start();
        }

        public void Stop()
        {
            UserActivityMonitor.UserInputActivity.MouseMove -= HandleMouseMoved;
            timer.Stop();
        }

        private void HandleMouseMoved(object sender, EventArgs e)
        {
            timer.Stop();
            timer.Start();
        }
    }
}