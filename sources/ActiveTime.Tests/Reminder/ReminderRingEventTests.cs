using System;
using System.Threading;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderTests
{
    [TestFixture]
    [Description("TestCase 3: Initialize - Start - Ring -> check is raised, check time, check status")]
    public class ReminderRingEventTests
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
        [Description("Checks if the Ring event is raised on time.")]
        [Timeout(200)]
        public void Start_int_RingTime()
        {
            const int ringMiliseconds = 100;

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                DateTime startTime;
                DateTime ringTime;

                reminder.Ring += new EventHandler<RingEventArgs>(delegate(object sender, RingEventArgs e)
                {
                    ringTime = DateTime.Now;
                    ringEvent.Set();
                });

                startTime = DateTime.Now;
                ringTime = DateTime.MinValue;

                reminder.Start(ringMiliseconds);

                if (ringEvent.WaitOne())
                {
                    Assert.That(ringTime - startTime, Is.EqualTo(TimeSpan.FromMilliseconds(ringMiliseconds)).Within(TimeSpan.FromMilliseconds(10)));
                }
            }
        }

        [Test]
        [Description("Checks if the Status is correctly set after the Ring event is raised.")]
        [Timeout(200)]
        public void Start_int_Ring_Status()
        {
            const int ringMiliseconds = 100;

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                ReminderStatus status = ReminderStatus.NotStarted;

                reminder.Ring += new EventHandler<RingEventArgs>(delegate(object sender, RingEventArgs e)
                {
                    if (sender != null)
                    {
                        status = ((Reminder)sender).Status;
                    }
                    ringEvent.Set();
                });

                reminder.Start(ringMiliseconds);

                if (ringEvent.WaitOne())
                {
                    Assert.That(status, Is.EqualTo(ReminderStatus.Stopping));
                }
            }
        }

        #endregion

        #region Start long

        [Test]
        [Description("Checks if the Ring event is raised on time.")]
        [Timeout(200)]
        public void Start_long_RingTime()
        {
            const long ringMiliseconds = 100;

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                DateTime startTime;
                DateTime ringTime;

                reminder.Ring += new EventHandler<RingEventArgs>((sender, e) =>
                {
                    ringTime = DateTime.Now;
                    ringEvent.Set();
                });

                startTime = DateTime.Now;
                ringTime = DateTime.MinValue;

                reminder.Start(ringMiliseconds);

                if (ringEvent.WaitOne())
                {
                    Assert.That(ringTime - startTime, Is.EqualTo(TimeSpan.FromMilliseconds(ringMiliseconds)).Within(TimeSpan.FromMilliseconds(10)));
                }
            }
        }

        [Test]
        [Description("Checks if the Status is correctly set after the Ring event is raised.")]
        [Timeout(200)]
        public void Start_long_Ring_Status()
        {
            const long ringMiliseconds = 100;

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                ReminderStatus status = ReminderStatus.NotStarted;

                reminder.Ring += new EventHandler<RingEventArgs>((sender, e) =>
                {
                    if (sender != null)
                    {
                        status = ((Reminder)sender).Status;
                    }
                    ringEvent.Set();
                });

                reminder.Start(ringMiliseconds);

                if (ringEvent.WaitOne())
                {
                    Assert.That(status, Is.EqualTo(ReminderStatus.Stopping));
                }
            }
        }

        #endregion

        #region Start uint

        [Test]
        [Description("Checks if the Ring event is raised on time.")]
        [Timeout(200)]
        public void Start_uint_RingTime()
        {
            const uint ringMiliseconds = 100;

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                DateTime startTime;
                DateTime ringTime;

                reminder.Ring += new EventHandler<RingEventArgs>((sender, e) =>
                {
                    ringTime = DateTime.Now;
                    ringEvent.Set();
                });

                startTime = DateTime.Now;
                ringTime = DateTime.MinValue;

                reminder.Start(ringMiliseconds);

                if (ringEvent.WaitOne())
                {
                    Assert.That(ringTime - startTime, Is.EqualTo(TimeSpan.FromMilliseconds(ringMiliseconds)).Within(TimeSpan.FromMilliseconds(10)));
                }
            }
        }

        [Test]
        [Description("Checks if the Status is correctly set after the Ring event is raised.")]
        [Timeout(200)]
        public void Start_uint_Ring_Status()
        {
            const uint ringMiliseconds = 100;

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                ReminderStatus status = ReminderStatus.NotStarted;

                reminder.Ring += new EventHandler<RingEventArgs>((sender, e) =>
                {
                    if (sender != null)
                    {
                        status = ((Reminder)sender).Status;
                    }
                    ringEvent.Set();
                });

                reminder.Start(ringMiliseconds);

                if (ringEvent.WaitOne())
                {
                    Assert.That(status, Is.EqualTo(ReminderStatus.Stopping));
                }
            }
        }

        #endregion

        #region Start TimeSpan

        [Test]
        [Description("Checks if the Ring event is raised on time.")]
        [Timeout(200)]
        public void Start_TimeSpan_RingTime()
        {
            TimeSpan ringMiliseconds = TimeSpan.FromMilliseconds(100);

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                DateTime ringTime;

                reminder.Ring += new EventHandler<RingEventArgs>((sender, e) =>
                {
                    ringTime = DateTime.Now;
                    ringEvent.Set();
                });

                DateTime startTime = DateTime.Now;
                ringTime = DateTime.MinValue;

                reminder.Start(ringMiliseconds);

                if (ringEvent.WaitOne())
                {
                    Assert.That(ringTime - startTime, Is.EqualTo(ringMiliseconds).Within(TimeSpan.FromMilliseconds(10)));
                }
            }
        }

        [Test]
        [Description("Checks if the Status is correctly set after the Ring event is raised.")]
        [Timeout(200)]
        public void Start_TimeSpan_Ring_Status()
        {
            TimeSpan ringMiliseconds = TimeSpan.FromMilliseconds(100);

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                ReminderStatus status = ReminderStatus.NotStarted;

                reminder.Ring += new EventHandler<RingEventArgs>((sender, e) =>
                {
                    if (sender != null)
                    {
                        status = ((Reminder) sender).Status;
                    }
                    ringEvent.Set();
                });

                reminder.Start(ringMiliseconds);

                if (ringEvent.WaitOne())
                {
                    Assert.That(status, Is.EqualTo(ReminderStatus.Stopping));
                }
            }
        }

        #endregion
    }
}
