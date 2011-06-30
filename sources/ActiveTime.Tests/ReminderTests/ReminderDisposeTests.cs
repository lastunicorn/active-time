using System;
using NUnit.Framework;
using System.Threading;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderTests
{
    [TestFixture]
    [Description("TestCase 8: Initialize - Start - Dispose -> check if stop, check nothing starts")]
    public class ReminderDisposeTests
    {
        private Reminder reminder;

        [SetUp]
        public void SetUp()
        {
            reminder = new Reminder();
            reminder.Dispose();
        }


        #region Dispose

        [Test]
        public void DisposeTwice()
        {
            reminder.Dispose();
        }

        #endregion

        #region Properties

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_SnoozeTime()
        {
            reminder.SnoozeTime = TimeSpan.FromMilliseconds(1);
        }

        #endregion

        #region Methods

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Start_int()
        {
            const int miliseconds = 100;
            reminder.Start(miliseconds);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Start_long()
        {
            const long miliseconds = 100;
            reminder.Start(miliseconds);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Start_uint()
        {
            const uint miliseconds = 100;
            reminder.Start(miliseconds);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Start_TimeSpan()
        {
            TimeSpan miliseconds = TimeSpan.FromMilliseconds(100);
            reminder.Start(miliseconds);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Stop()
        {
            reminder.Stop();
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Reset()
        {
            reminder.Reset();
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_WaitUntilRing()
        {
            reminder.WaitUntilRing();
        }

        #endregion

        #region Stop

        [Test]
        public void Dispose_StopAll()
        {
            using (Reminder reminder = new Reminder())
            {
                reminder.Start(100);

                Thread.Sleep(50);

                reminder.Dispose();

                Assert.That(reminder.Status, Is.EqualTo(ReminderStatus.NotStarted));
            }
        }

        #endregion
    }
}
