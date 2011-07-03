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
    }
}
