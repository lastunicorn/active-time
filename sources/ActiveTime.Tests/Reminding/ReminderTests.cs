// ActiveTime
// Copyright (C) 2011 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Threading;
using DustInTheWind.ActiveTime.Common.Reminding;
using DustInTheWind.ActiveTime.Reminding.Services;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderTests
{
    [TestFixture]
    public class ReminderTests
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

        #region Initialization - TestCase 1: Initialize -> check status

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

        #endregion

        #region Start - TestCase 2: Initialize - Start -> check status

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

        #endregion

        #region Ring - TestCase 3: Initialize - Start - Ring -> check is raised, check time, check status

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
                    Assert.That(ringTime - startTime, Is.EqualTo(TimeSpan.FromMilliseconds(ringMiliseconds)).Within(TimeSpan.FromMilliseconds(20)));
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
                    Assert.That(ringTime - startTime, Is.EqualTo(TimeSpan.FromMilliseconds(ringMiliseconds)).Within(TimeSpan.FromMilliseconds(20)));
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
                    Assert.That(ringTime - startTime, Is.EqualTo(TimeSpan.FromMilliseconds(ringMiliseconds)).Within(TimeSpan.FromMilliseconds(20)));
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
                    Assert.That(ringTime - startTime, Is.EqualTo(ringMiliseconds).Within(TimeSpan.FromMilliseconds(20)));
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

        #endregion

        #region Stop - TestCase 4: Initialize - Start - Stop -> check status, check not ring

        [Test]
        [Description("Checks if the Ring event is raised on time.")]
        [Timeout(200)]
        public void Stop_Status()
        {
            const int ringMiliseconds = 100;

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                reminder.Ring += new EventHandler<RingEventArgs>((sender, e) => ringEvent.Set());

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

        #endregion

        #region Stopped Event - TestCase 5: Initialize - Start - Stop - Stopped event -> check is raised, check status

        [Test]
        [Description("Checks if the Ring event is raised on time.")]
        [Timeout(200)]
        public void StopEvent_Status()
        {
            const int ringMiliseconds = 100;

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                ReminderStatus status = ReminderStatus.NotStarted;
                bool stoppedEventWasRaised = false;

                reminder.Ring += new EventHandler<RingEventArgs>((sender, e) => ringEvent.Set());

                reminder.Stopped += new EventHandler((sender, e) =>
                {
                    if (sender != null)
                    {
                        status = ((Reminder)sender).Status;
                    }
                    stoppedEventWasRaised = true;
                });

                reminder.Start(ringMiliseconds);

                Thread.Sleep(50);

                reminder.Stop();

                if (stoppedEventWasRaised)
                {
                    Assert.That(status, Is.EqualTo(ReminderStatus.NotStarted));
                }
                else
                {
                    Assert.Fail("The Stopped event was not raised.");
                }
            }
        }

        #endregion

        #region Snooze - TestCase 6: Initialize - Start - Snooze -> check status, check not ring, check ring after snooze

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

        #endregion

        #region WaitUntilRing - TestCase 7: Initialize - Start - WaitUntilRing -> check status, check ring, check time

        [Test]
        [Timeout(200)]
        public void WaitUntilRing_ReturnValue_Ring()
        {
            const int ringMiliseconds = 100;

            reminder.Start(ringMiliseconds);
            bool isRing = reminder.WaitUntilRing();

            Assert.That(isRing, Is.EqualTo(true));
        }

        [Test]
        [Timeout(300)]
        public void WaitUntilRing_ReturnValue_Stop()
        {
            const int ringMiliseconds = 200;
            const int stopMiliseconds = 100;

            reminder.Start(ringMiliseconds);

            // Set the timer to stop the reminder.
            new Timer(new TimerCallback(o => reminder.Stop()), null, stopMiliseconds, -1);

            bool isRing = reminder.WaitUntilRing();

            Assert.That(isRing, Is.EqualTo(false));
        }

        [Test]
        [Timeout(200)]
        public void WaitUntilRing_Time_Ring()
        {
            const int ringMiliseconds = 100;

            DateTime startTime = DateTime.Now;

            reminder.Start(ringMiliseconds);
            reminder.WaitUntilRing();

            DateTime ringTime = DateTime.Now;

            Assert.That(ringTime - startTime, Is.EqualTo(TimeSpan.FromMilliseconds(ringMiliseconds)).Within(TimeSpan.FromMilliseconds(20)));
        }

        [Test]
        [Timeout(300)]
        public void WaitUntilRing_Time_Stop()
        {
            const int ringMiliseconds = 200;
            const int stopMiliseconds = 100;

            DateTime startTime = DateTime.Now;

            reminder.Start(ringMiliseconds);

            // Set the timer to stop the reminder.
            new Timer(new TimerCallback(o => reminder.Stop()), null, stopMiliseconds, -1);

            reminder.WaitUntilRing();

            DateTime ringTime = DateTime.Now;

            Assert.That(ringTime - startTime, Is.EqualTo(TimeSpan.FromMilliseconds(stopMiliseconds)).Within(TimeSpan.FromMilliseconds(20)));
        }

        #endregion

        #region Dispose - TestCase 8: Initialize - Start - Dispose -> check if stop, check nothing starts

        #region Dispose

        [Test]
        public void DisposeTwice()
        {
            reminder.Dispose();
            reminder.Dispose();
        }

        #endregion

        #region Properties

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_SnoozeTime()
        {
            reminder.Dispose();
            reminder.SnoozeTime = TimeSpan.FromMilliseconds(1);
        }

        #endregion

        #region Methods

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Start_int()
        {
            reminder.Dispose();
            const int miliseconds = 100;
            reminder.Start(miliseconds);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Start_long()
        {
            reminder.Dispose();
            const long miliseconds = 100;
            reminder.Start(miliseconds);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Start_uint()
        {
            reminder.Dispose();
            const uint miliseconds = 100;
            reminder.Start(miliseconds);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Start_TimeSpan()
        {
            reminder.Dispose();
            TimeSpan miliseconds = TimeSpan.FromMilliseconds(100);
            reminder.Start(miliseconds);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Stop()
        {
            reminder.Dispose();
            reminder.Stop();
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Reset()
        {
            reminder.Dispose();
            reminder.Reset();
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_WaitUntilRing()
        {
            reminder.Dispose();
            reminder.WaitUntilRing();
        }

        #endregion

        #region Stop

        [Test]
        public void Dispose_StopaAll()
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

        #endregion
    }
}
