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
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.ShellNavigation;
using DustInTheWind.ActiveTime.ReminderModule.Reminding;
using DustInTheWind.ActiveTime.ReminderModule.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderModule.Services.PauseReminderTests
{
    [TestFixture]
    public class StartMonitoringTests
    {
        private Mock<IRecorderService> recorderService;
        private Mock<IShellNavigator> shellNavigator;
        private Mock<IReminder> reminder;
        private PauseReminder pauseReminder;

        [SetUp]
        public void SetUp()
        {
            recorderService = new Mock<IRecorderService>();
            shellNavigator = new Mock<IShellNavigator>();
            reminder = new Mock<IReminder>();

            pauseReminder = new PauseReminder(recorderService.Object, shellNavigator.Object, reminder.Object);
        }

        [Test]
        public void starts_reminder_if_recorder_is_already_started()
        {
            recorderService.Setup(x => x.State).Returns(RecorderState.Running);
            reminder.Setup(x => x.Start(TimeSpan.FromHours(1)));

            pauseReminder.StartMonitoring();

            recorderService.Verify(x => x.State, Times.Once());
            reminder.Verify(x => x.Start(TimeSpan.FromHours(1)), Times.Once());
        }

        [Test]
        public void called_second_time_does_nothing()
        {
            recorderService.Setup(x => x.State).Returns(RecorderState.Running);

            pauseReminder.StartMonitoring();
            pauseReminder.StartMonitoring();

            recorderService.Verify(x => x.State, Times.Once());
            reminder.Verify(x => x.Start(TimeSpan.FromHours(1)), Times.Once());
        }
    }
}
