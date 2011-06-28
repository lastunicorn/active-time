using System;
using System.Threading;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderTests
{
    [TestFixture]
    [Description("TestCase 4: Initialize - Start - Stop -> check status, check not ring")]
    public class ReminderStopTests
    {
        private Reminder reminder;

        [SetUp]
        public void SetUp()
        {
            reminder = new Reminder();
        }

        [TearDown]
        public void TearDown()
        {
            reminder.Dispose();
        }

        [Test]
        [Description("Checks if the Ring event is raised on time.")]
        [Timeout(200)]
        public void Stop_Status()
        {
            int ringMiliseconds = 100;

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                reminder.Ring += new EventHandler<RingEventArgs>(delegate(object sender, RingEventArgs e)
                {
                    ringEvent.Set();
                });

                reminder.Start(ringMiliseconds);
                
                Thread.Sleep(50);

                reminder.Stop();

                Assert.That(reminder.Status, Is.EqualTo(ReminderStatus.NotStarted));

                if (ringEvent.WaitOne(100))
                {
                    Assert.Fail("The Ring event was raised after the Stop method was called.");
                }
            }
        }
    }
}
