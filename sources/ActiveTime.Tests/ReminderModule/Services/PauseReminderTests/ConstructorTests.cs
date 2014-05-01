﻿// ActiveTime
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
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.ShellNavigation;
using DustInTheWind.ActiveTime.ReminderModule.Reminding;
using DustInTheWind.ActiveTime.ReminderModule.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderModule.Services.PauseReminderTests
{
    [TestFixture]
    public class ConstructorTests
    {
        private Mock<IRecorderService> recorderService;
        private Mock<IShellNavigator> shellNavigator;
        private Mock<IReminder> reminder;

        [SetUp]
        public void SetUp()
        {
            recorderService = new Mock<IRecorderService>();
            shellNavigator = new Mock<IShellNavigator>();
            reminder = new Mock<IReminder>();
        }

        [Test]
        public void successfully_initialized()
        {
            new PauseReminder(recorderService.Object, shellNavigator.Object, reminder.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void throws_if_recorderService_is_null()
        {
            new PauseReminder(null, shellNavigator.Object, reminder.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void throws_if_shellNavigator_is_null()
        {
            new PauseReminder(recorderService.Object, null, reminder.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void throws_if_reminder_is_null()
        {
            new PauseReminder(recorderService.Object, shellNavigator.Object, null);
        }

        [Test]
        public void PauseInterval_initial_value()
        {
            PauseReminder pauseReminder = new PauseReminder(recorderService.Object, shellNavigator.Object, reminder.Object);

            Assert.That(pauseReminder.PauseInterval, Is.EqualTo(TimeSpan.FromHours(1)));
        }

        [Test]
        public void SnoozeInterval_initial_value()
        {
            PauseReminder pauseReminder = new PauseReminder(recorderService.Object, shellNavigator.Object, reminder.Object);

            Assert.That(pauseReminder.SnoozeInterval, Is.EqualTo(TimeSpan.FromMinutes(3)));
        }
    }
}
