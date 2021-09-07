// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
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
using DustInTheWind.ActiveTime.Reminder.Module.Reminding;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderModule.Reminding.ReminderTests
{
    /// <summary>
    /// Start - TestCase 2: Initialize - Start -> check status
    /// </summary>
    [TestFixture]
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "The disposable objects are disposed in the TearDown method.")]
    public class StartTests
    {
        private Reminder.Module.Reminding.Reminder reminder;

        [SetUp]
        public void SetUp()
        {
            reminder = new Reminder.Module.Reminding.Reminder();
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

            Assert.That(reminder.StartTime, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromMilliseconds(TestConstants.TimeErrorAccepted)));
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

            Assert.That(reminder.StartTime, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromMilliseconds(TestConstants.TimeErrorAccepted)));
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

            Assert.That(reminder.StartTime, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromMilliseconds(TestConstants.TimeErrorAccepted)));
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

            Assert.That(reminder.StartTime, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromMilliseconds(TestConstants.TimeErrorAccepted)));
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
