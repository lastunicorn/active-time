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
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI.ShellNavigation;
using DustInTheWind.ActiveTime.Logging;
using DustInTheWind.ActiveTime.Recording;
using DustInTheWind.ActiveTime.ReminderModule.Reminding;
using DustInTheWind.ActiveTime.ReminderModule.Services;
using DustInTheWind.ActiveTime.Services;
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
        private Mock<ILogger> logger;

        [SetUp]
        public void SetUp()
        {
            recorderService = new Mock<IRecorderService>();
            shellNavigator = new Mock<IShellNavigator>();
            reminder = new Mock<IReminder>();
            logger = new Mock<ILogger>();
        }

        [Test]
        public void throws_if_recorderService_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new PauseReminder(null, shellNavigator.Object, reminder.Object, logger.Object));
        }

        [Test]
        public void throws_if_shellNavigator_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new PauseReminder(recorderService.Object, null, reminder.Object, logger.Object));
        }

        [Test]
        public void throws_if_reminder_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new PauseReminder(recorderService.Object, shellNavigator.Object, null, logger.Object));
        }

        [Test]
        public void PauseInterval_initial_value_is_1_hour()
        {
            PauseReminder pauseReminder = new PauseReminder(recorderService.Object, shellNavigator.Object, reminder.Object, logger.Object);

            Assert.That(pauseReminder.PauseInterval, Is.EqualTo(TimeSpan.FromHours(1)));
        }

        [Test]
        public void SnoozeInterval_initial_value_is_3_minutes()
        {
            PauseReminder pauseReminder = new PauseReminder(recorderService.Object, shellNavigator.Object, reminder.Object, logger.Object);

            Assert.That(pauseReminder.SnoozeInterval, Is.EqualTo(TimeSpan.FromMinutes(3)));
        }
    }
}
