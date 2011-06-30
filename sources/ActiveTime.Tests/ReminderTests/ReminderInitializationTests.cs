using System;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderTests
{
    [TestFixture]
    [Description("TestCase 1: Initialize -> check status")]
    public class ReminderStartTests
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
        public void Initialization()
        {
        }

        [Test]
        public void InitialState_Status()
        {
            Assert.That(reminder.Status, Is.EqualTo(ReminderStatus.NotStarted));
        }

        [Test]
        public void InitialState_StartTime()
        {
            Assert.That(reminder.StartTime, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void InitialState_SnoozeTime()
        {
            Assert.That(reminder.SnoozeTime, Is.EqualTo(TimeSpan.FromMinutes(1)));
        }
    }
}
