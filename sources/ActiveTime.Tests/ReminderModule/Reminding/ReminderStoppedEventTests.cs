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
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderTests
{
    [TestFixture]
    [Description("TestCase 5: Initialize - Start - Stop - Stopped event -> check is raised, check status")]
    public class ReminderStoppedEventTests
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
                                                                 status = ((Reminder) sender).Status;
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
    }
}
