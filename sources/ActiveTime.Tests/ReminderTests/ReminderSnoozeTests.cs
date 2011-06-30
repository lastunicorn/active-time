using System;
using NUnit.Framework;
using System.Threading;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderTests
{
    [TestFixture]
    [Description("TestCase 6: Initialize - Start - Snooze -> check status, check not ring, check ring after snooze")]
    public class ReminderSnoozeTests
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

        /// <summary>
        /// Start 100ms - Snooze default -> check status
        /// </summary>
        [Test]
        [Timeout(400)]
        public void SnoozeDefault_Status()
        {
            const int ringMiliseconds = 100;

            reminder.SnoozeTime = TimeSpan.FromMilliseconds(200);

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                reminder.Ring += new EventHandler<RingEventArgs>(delegate(object sender, RingEventArgs e)
                {
                    e.Snooze = true;
                    ringEvent.Set();
                });

                reminder.Start(ringMiliseconds);

                if (ringEvent.WaitOne())
                {
                    // Wait a while for the snooze process to start.
                    Thread.Sleep(10);

                    Assert.That(reminder.Status, Is.EqualTo(ReminderStatus.Snooze));
                }
            }
        }

        /// <summary>
        /// Start 100ms - Snooze default(200ms) - Ring -> check time
        /// </summary>
        [Test]
        [Timeout(400)]
        public void SnoozeDefault_Time()
        {
            const int ringMiliseconds = 100;

            reminder.SnoozeTime = TimeSpan.FromMilliseconds(200);

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                int ringCount = 0;
                DateTime snoozeStartTime = DateTime.MinValue;
                DateTime snoozeEndTime = DateTime.MinValue;

                reminder.Ring += new EventHandler<RingEventArgs>(delegate(object sender, RingEventArgs e)
                {
                    ringCount++;

                    switch (ringCount)
                    {
                        case 1:
                            e.Snooze = true;
                            snoozeStartTime = DateTime.Now;
                            break;

                        case 2:
                            snoozeEndTime = DateTime.Now;
                            ringEvent.Set();
                            break;

                        default:
                            break;
                    }

                });

                reminder.Start(ringMiliseconds);

                if (ringEvent.WaitOne())
                {
                    Assert.That(snoozeEndTime - snoozeStartTime, Is.EqualTo(reminder.SnoozeTime).Within(TimeSpan.FromMilliseconds(50)));
                }
            }
        }

        /// <summary>
        /// Start 100ms - Snooze 150 - Ring -> check time
        /// </summary>
        [Test]
        [Timeout(400)]
        public void Snooze_Time()
        {
            const int ringMiliseconds = 100;
            const int snoozeMiliseconds = 150;

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                int ringCount = 0;
                DateTime snoozeStartTime = DateTime.MinValue;
                DateTime snoozeEndTime = DateTime.MinValue;

                reminder.Ring += new EventHandler<RingEventArgs>(delegate(object sender, RingEventArgs e)
                {
                    ringCount++;

                    switch (ringCount)
                    {
                        case 1:
                            e.Snooze = true;
                            e.SnoozeTime = TimeSpan.FromMilliseconds(snoozeMiliseconds);
                            snoozeStartTime = DateTime.Now;
                            break;

                        case 2:
                            snoozeEndTime = DateTime.Now;
                            ringEvent.Set();
                            break;

                        default:
                            break;
                    }

                });

                reminder.Start(ringMiliseconds);

                if (ringEvent.WaitOne())
                {
                    Assert.That(snoozeEndTime - snoozeStartTime, Is.EqualTo(TimeSpan.FromMilliseconds(snoozeMiliseconds)).Within(TimeSpan.FromMilliseconds(50)));
                }
            }
        }
    }
}
