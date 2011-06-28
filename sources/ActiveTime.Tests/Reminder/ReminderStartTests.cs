using System;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderTests
{
    [TestFixture]
    [Description("TestCase 2: Initialize - Start -> check status")]
    public class ReminderInitializationTests
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

        #region Start int

        [Test]
        [Description("Tests if the StartTime value is set correctly by the Start method.")]
        public void Start_int_StartTime()
        {
            const int miliseconds = 1000;
            reminder.Start(miliseconds);

            Assert.That(reminder.StartTime, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromMilliseconds(100)));
        }

        [Test]
        [Description("Tests if the Status value is set correctly by the Start method.")]
        public void Start_int_Status()
        {
            const int miliseconds = 1000;
            reminder.Start(miliseconds);

            Assert.That(reminder.Status, Is.EqualTo(ReminderStatus.Running));
        }

        #endregion

        #region Start long

        [Test]
        [Description("Tests if the StartTime value is set correctly by the Start method.")]
        public void Start_long_StartTime()
        {
            const long miliseconds = 1000;
            reminder.Start(miliseconds);

            Assert.That(reminder.StartTime, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromMilliseconds(100)));
        }

        [Test]
        [Description("Tests if the Status value is set correctly by the Start method.")]
        public void Start_long_Status()
        {
            const long miliseconds = 1000;
            reminder.Start(miliseconds);

            Assert.That(reminder.Status, Is.EqualTo(ReminderStatus.Running));
        }

        #endregion

        #region Start uint

        [Test]
        [Description("Tests if the StartTime value is set correctly by the Start method.")]
        public void Start_uint_StartTime()
        {
            const uint miliseconds = 1000;
            reminder.Start(miliseconds);

            Assert.That(reminder.StartTime, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromMilliseconds(100)));
        }

        [Test]
        [Description("Tests if the Status value is set correctly by the Start method.")]
        public void Start_uint_Status()
        {
            const uint miliseconds = 1000;
            reminder.Start(miliseconds);

            Assert.That(reminder.Status, Is.EqualTo(ReminderStatus.Running));
        }

        #endregion

        #region Start TimeSpan

        [Test]
        [Description("Tests if the StartTime value is set correctly by the Start method.")]
        public void Start_TimeSpan_StartTime()
        {
            TimeSpan time = TimeSpan.FromSeconds(1);
            reminder.Start(time);

            Assert.That(reminder.StartTime, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromMilliseconds(100)));
        }

        [Test]
        [Description("Tests if the Status value is set correctly by the Start method.")]
        public void Start_TimeSpan_Status()
        {
            TimeSpan time = TimeSpan.FromSeconds(1);
            reminder.Start(time);

            Assert.That(reminder.Status, Is.EqualTo(ReminderStatus.Running));
        }

        #endregion
    }
}
