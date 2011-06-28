using System;
using System.Threading;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderTests
{
    [TestFixture]
    [Description("TestCase 7: Initialize - Start - WaitUntilRing -> check status, check ring, check time")]
    public class ReminderWaitUntilRingTests
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
        [Timeout(200)]
        public void WaitUntilRing_ReturnValue_Ring()
        {
            int ringMiliseconds = 100;

            reminder.Start(ringMiliseconds);
            bool isRing = reminder.WaitUntilRing();

            Assert.That(isRing, Is.EqualTo(true));
        }

        [Test]
        [Timeout(300)]
        public void WaitUntilRing_ReturnValue_Stop()
        {
            int ringMiliseconds = 200;
            int stopMiliseconds = 100;

            reminder.Start(ringMiliseconds);

            // Set the timer to stop the reminder.
            new Timer(new TimerCallback(delegate(object o) { reminder.Stop(); }), null, stopMiliseconds, -1);

            bool isRing = reminder.WaitUntilRing();

            Assert.That(isRing, Is.EqualTo(false));
        }

        [Test]
        [Timeout(200)]
        public void WaitUntilRing_Time_Ring()
        {
            int ringMiliseconds = 100;

            DateTime startTime = DateTime.Now;

            reminder.Start(ringMiliseconds);
            reminder.WaitUntilRing();

            DateTime ringTime = DateTime.Now;

            Assert.That(ringTime - startTime, Is.EqualTo(TimeSpan.FromMilliseconds(ringMiliseconds)).Within(TimeSpan.FromMilliseconds(10)));
        }

        [Test]
        [Timeout(300)]
        public void WaitUntilRing_Time_Stop()
        {
            int ringMiliseconds = 200;
            int stopMiliseconds = 100;

            DateTime startTime = DateTime.Now;

            reminder.Start(ringMiliseconds);

            // Set the timer to stop the reminder.
            new Timer(new TimerCallback(delegate(object o) { reminder.Stop(); }), null, stopMiliseconds, -1);

            reminder.WaitUntilRing();

            DateTime ringTime = DateTime.Now;

            Assert.That(ringTime - startTime, Is.EqualTo(TimeSpan.FromMilliseconds(stopMiliseconds)).Within(TimeSpan.FromMilliseconds(10)));
        }
    }
}
