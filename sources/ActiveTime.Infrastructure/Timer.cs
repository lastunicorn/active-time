using System;
using System.Timers;

namespace DustInTheWind.ActiveTime.Infrastructure
{
    public class Timer : ITimer
    {
        private readonly System.Timers.Timer timer;

        public TimeSpan Interval
        {
            get => TimeSpan.FromMilliseconds(timer.Interval);
            set => timer.Interval = value.TotalMilliseconds;
        }

        public event EventHandler Tick;

        public Timer()
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += HandleTimerElapsed;
        }

        private void HandleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            OnTick();
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        protected virtual void OnTick()
        {
            Tick?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                timer?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}