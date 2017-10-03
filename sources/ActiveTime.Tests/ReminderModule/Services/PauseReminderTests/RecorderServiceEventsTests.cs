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
using DustInTheWind.ActiveTime.Reminder.Module.Reminding;
using DustInTheWind.ActiveTime.Reminder.Module.Services;
using DustInTheWind.ActiveTime.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderModule.Services.PauseReminderTests
{
    [TestFixture]
    public class RecorderServiceEventsTests
    {
        private Mock<IRecorderService> recorderService;
        private Mock<IShellNavigator> shellNavigator;
        private Mock<IReminder> reminder;
        private PauseReminder pauseReminder;
        private Mock<ILogger> logger;

        [SetUp]
        public void SetUp()
        {
            recorderService = new Mock<IRecorderService>();
            shellNavigator = new Mock<IShellNavigator>();
            reminder = new Mock<IReminder>();
            logger = new Mock<ILogger>();

            pauseReminder = new PauseReminder(recorderService.Object, shellNavigator.Object, reminder.Object, logger.Object);
        }

        [Test]
        public void Reminder_is_started_when_Recorder_starts()
        {
            pauseReminder.StartMonitoring();
            reminder.Setup(x => x.Start(It.IsAny<TimeSpan>()));

            recorderService.Raise(x => x.Started += null, EventArgs.Empty);

            reminder.VerifyAll();
        }

        [Test]
        public void Reminder_is_stopped_when_Recorder_stops()
        {
            pauseReminder.StartMonitoring();
            reminder.Setup(x => x.Stop());

            recorderService.Raise(x => x.Stopped += null, EventArgs.Empty);

            reminder.VerifyAll();
        }
    }
}
