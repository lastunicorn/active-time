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
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using DustInTheWind.ActiveTime.ReminderModule.Reminding;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderModule.Reminding.ReminderTests
{
    /// <summary>
    /// Snooze - TestCase 6: Initialize - Start - Snooze -> check status, check not ring, check ring after snooze
    /// </summary>
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class SnoozeTests
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

            reminder.DefaultSnoozeTime = TimeSpan.FromMilliseconds(200);

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                reminder.Ring += (sender, e) =>
                {
                    e.Snooze = true;
                    ringEvent.Set();
                };

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

            reminder.DefaultSnoozeTime = TimeSpan.FromMilliseconds(200);

            using (ManualResetEvent ringEvent = new ManualResetEvent(false))
            {
                int ringCount = 0;
                DateTime snoozeStartTime = DateTime.MinValue;
                DateTime snoozeEndTime = DateTime.MinValue;

                reminder.Ring += delegate(object sender, RingEventArgs e)
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
                    }

                };

                reminder.Start(ringMiliseconds);

                if (ringEvent.WaitOne())
                {
                    TimeSpan actual = snoozeEndTime - snoozeStartTime;
                    TimeSpan expected = reminder.DefaultSnoozeTime;

                    TimeSpan acceptedError = TimeSpan.FromMilliseconds(TestConstants.TimerDelayAccepted);
                    Assert.That(actual, Is.EqualTo(expected).Within(acceptedError));
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

                reminder.Ring += delegate(object sender, RingEventArgs e)
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
                    }

                };

                reminder.Start(ringMiliseconds);

                if (ringEvent.WaitOne())
                {
                    TimeSpan actual = snoozeEndTime - snoozeStartTime;
                    TimeSpan expected = TimeSpan.FromMilliseconds(snoozeMiliseconds);

                    TimeSpan acceptedError = TimeSpan.FromMilliseconds(TestConstants.TimerDelayAccepted);
                    Assert.That(actual, Is.EqualTo(expected).Within(acceptedError));
                }
            }
        }
    }
}
