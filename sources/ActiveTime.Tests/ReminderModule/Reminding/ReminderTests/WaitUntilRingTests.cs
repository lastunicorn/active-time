// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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
    /// WaitUntilRing - TestCase 7: Initialize - Start - WaitUntilRing -> check status, check ring, check time
    /// </summary>
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class WaitUntilRingTests
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
        [Timeout(100 + TestConstants.TimerDelayAccepted)]
        public void WaitUntilRing_ReturnValue_Ring()
        {
            const int ringMiliseconds = 100;

            reminder.Start(ringMiliseconds);
            bool isRing = reminder.WaitUntilRing();

            Assert.That(isRing, Is.EqualTo(true));
        }

        [Test]
        [Timeout(200 + TestConstants.TimerDelayAccepted)]
        public void WaitUntilRing_ReturnValue_Stop()
        {
            const int ringMiliseconds = 200;
            const int stopMiliseconds = 100;

            reminder.Start(ringMiliseconds);

            // Set the timer to stop the reminder.
            new Timer(o => reminder.Stop(), null, stopMiliseconds, -1);

            bool isRing = reminder.WaitUntilRing();

            Assert.That(isRing, Is.EqualTo(false));
        }

        [Test]
        [Timeout(100 + TestConstants.TimerDelayAccepted)]
        public void WaitUntilRing_Time_Ring()
        {
            const int ringMiliseconds = 100;

            DateTime startTime = DateTime.Now;

            reminder.Start(ringMiliseconds);
            reminder.WaitUntilRing();

            DateTime ringTime = DateTime.Now;

            TimeSpan actual = ringTime - startTime;
            TimeSpan expected = TimeSpan.FromMilliseconds(ringMiliseconds);

            Assert.That(actual, Is.EqualTo(expected).Within(TimeSpan.FromMilliseconds(TestConstants.TimeErrorAccepted)));
        }

        [Test]
        [Timeout(200 + TestConstants.TimerDelayAccepted)]
        public void WaitUntilRing_Time_Stop()
        {
            const int ringMiliseconds = 200;
            const int stopMiliseconds = 100;

            DateTime startTime = DateTime.Now;

            reminder.Start(ringMiliseconds);

            // Set the timer to stop the reminder.
            new Timer(o => reminder.Stop(), null, stopMiliseconds, -1);

            reminder.WaitUntilRing();

            DateTime ringTime = DateTime.Now;

            TimeSpan actual = ringTime - startTime;
            TimeSpan expected = TimeSpan.FromMilliseconds(stopMiliseconds);

            TimeSpan acceptedError = TimeSpan.FromMilliseconds(TestConstants.TimeErrorAccepted);
            Assert.That(actual, Is.EqualTo(expected).Within(acceptedError));
        }
    }
}
