using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit
{
    internal class TimeAssertion : IDisposable
    {
        private readonly ManualResetEventSlim stampingManualResetEvent = new ManualResetEventSlim(false);

        public TimeSpan Timeout { get; set; } = TimeSpan.FromMilliseconds(200);

        public TimeSpan MeasurementLag { get; set; } = TimeSpan.FromMilliseconds(20);

        public void Run(TimeSpan expected, Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            action();
            bool success = stampingManualResetEvent.Wait(Timeout);

            if (!success)
                throw new AssertionException("Assertion timeout.");

            Assert.That(stopwatch.Elapsed, Is.EqualTo(expected).Within(MeasurementLag));
        }

        public void Signal()
        {
            stampingManualResetEvent.Set();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                stampingManualResetEvent?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}