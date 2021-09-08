using System;

namespace DustInTheWind.ActiveTime.Infrastructure
{
    public interface ITimer : IDisposable
    {
        TimeSpan Interval { get; set; }

        event EventHandler Tick;

        void Start();

        void Stop();
    }
}