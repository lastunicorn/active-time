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
using NUnit.Framework;
using System.Threading;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderTests
{
    [TestFixture]
    [Description("TestCase 8: Initialize - Start - Dispose -> check if stop, check nothing starts")]
    public class ReminderDisposeTests
    {
        private Reminder reminder;

        [SetUp]
        public void SetUp()
        {
            reminder = new Reminder();
            reminder.Dispose();
        }


        #region Dispose

        [Test]
        public void DisposeTwice()
        {
            reminder.Dispose();
        }

        #endregion

        #region Properties

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_SnoozeTime()
        {
            reminder.SnoozeTime = TimeSpan.FromMilliseconds(1);
        }

        #endregion

        #region Methods

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Start_int()
        {
            const int miliseconds = 100;
            reminder.Start(miliseconds);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Start_long()
        {
            const long miliseconds = 100;
            reminder.Start(miliseconds);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Start_uint()
        {
            const uint miliseconds = 100;
            reminder.Start(miliseconds);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Start_TimeSpan()
        {
            TimeSpan miliseconds = TimeSpan.FromMilliseconds(100);
            reminder.Start(miliseconds);
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Stop()
        {
            reminder.Stop();
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_Reset()
        {
            reminder.Reset();
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Dispose_WaitUntilRing()
        {
            reminder.WaitUntilRing();
        }

        #endregion

        #region Stop

        [Test]
        public void Dispose_StopAll()
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
    }
}
