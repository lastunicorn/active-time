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
    /// Dispose - TestCase 8: Initialize - Start - Dispose -> check if stop, check nothing starts
    /// </summary>
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class DisposeTests
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

        #region Dispose

        [Test]
        public void DisposeTwice()
        {
            reminder.Dispose();
            reminder.Dispose();
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
        public void Dispose_StopAll()
        {
            Reminder reminder = new Reminder();

            reminder.Start(100);

            Thread.Sleep(50);

            reminder.Dispose();

            Assert.That(reminder.Status, Is.EqualTo(ReminderStatus.NotStarted));
        }

        #endregion
    }
}
