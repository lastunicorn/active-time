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

using System.Collections.Generic;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Common.UI.ShellNavigation;
using DustInTheWind.ActiveTime.Logging;
using DustInTheWind.ActiveTime.Recording;
using DustInTheWind.ActiveTime.ReminderModule.Inhibitors;
using DustInTheWind.ActiveTime.ReminderModule.Reminding;
using DustInTheWind.ActiveTime.ReminderModule.Services;
using DustInTheWind.ActiveTime.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.ReminderModule.Services.PauseReminderTests
{
    [TestFixture]
    public class ReminderEventsTests
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
        public void message_is_displayed_when_reminder_rings()
        {
            pauseReminder.StartMonitoring();

            reminder.Raise(x => x.Ring += null, new RingEventArgs());

            shellNavigator.Verify(x => x.Navigate(ShellNames.MessageShell, It.IsAny<Dictionary<string, object>>()), Times.Once());
        }

        [Test]
        public void message_is_not_displayed_if_reminder_rings_but_inhibitor_does_not_allow()
        {
            Mock<IReminderInhibitor> inhibitor = new Mock<IReminderInhibitor>();
            inhibitor
                .SetupGet(x => x.Allow)
                .Returns(false);
            pauseReminder.Inhibitors.Add(inhibitor.Object);
            pauseReminder.StartMonitoring();

            reminder.Raise(x => x.Ring += null, new RingEventArgs());

            shellNavigator.Verify(x => x.Navigate(ShellNames.MessageShell, It.IsAny<Dictionary<string, object>>()), Times.Never());
        }

        [Test]
        public void message_is_displayed_if_reminder_rings_and_inhibitor_allows()
        {
            Mock<IReminderInhibitor> inhibitor = new Mock<IReminderInhibitor>();
            inhibitor
                .SetupGet(x => x.Allow)
                .Returns(true);
            pauseReminder.Inhibitors.Add(inhibitor.Object);
            pauseReminder.StartMonitoring();

            reminder.Raise(x => x.Ring += null, new RingEventArgs());

            shellNavigator.Verify(x => x.Navigate(ShellNames.MessageShell, It.IsAny<Dictionary<string, object>>()), Times.Once());
        }
    }
}
