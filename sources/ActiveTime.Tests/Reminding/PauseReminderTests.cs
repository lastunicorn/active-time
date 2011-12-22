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
using System.Collections.Generic;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Reminding;
using DustInTheWind.ActiveTime.Common.ShellNavigation;
using DustInTheWind.ActiveTime.ReminderModule.Services;
using Moq;
using NUnit.Framework;
using System.Threading;

namespace DustInTheWind.ActiveTime.UnitTests.Reminding
{
    [TestFixture]
    public class PauseReminderTests
    {
        private Mock<IRecorderService> recorderServiceMock;
        private Mock<IShellNavigator> shellNavigatorMock;
        private Mock<IReminder> reminderMock;

        [SetUp]
        public void SetUp()
        {
            recorderServiceMock = new Mock<IRecorderService>();
            shellNavigatorMock = new Mock<IShellNavigator>();
            reminderMock = new Mock<IReminder>();
        }

        private PauseReminder CreatePauseReminder()
        {
            return new PauseReminder(recorderServiceMock.Object, shellNavigatorMock.Object, reminderMock.Object);
        }

        #region Constructor Tests

        [Test]
        public void Constructor_run_ok()
        {
            CreatePauseReminder();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_throws_if_recorderService_is_null()
        {
            new PauseReminder(null, shellNavigatorMock.Object, reminderMock.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_throws_if_shellNavigator_is_null()
        {
            new PauseReminder(recorderServiceMock.Object, null, reminderMock.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_throws_if_reminder_is_null()
        {
            new PauseReminder(recorderServiceMock.Object, shellNavigatorMock.Object, null);
        }

        #endregion

        [Test]
        public void Reminder_is_started_when_Recorder_starts()
        {
            PauseReminder pauseReminder = CreatePauseReminder();
            pauseReminder.StartMonitoring();
            reminderMock.Setup(x => x.Start(It.IsAny<TimeSpan>()));

            recorderServiceMock.Raise(x => x.Started += null, EventArgs.Empty);

            reminderMock.VerifyAll();
        }

        [Test]
        public void Reminder_is_stopped_when_Recorder_stops()
        {
            PauseReminder pauseReminder = CreatePauseReminder();
            pauseReminder.StartMonitoring();
            reminderMock.Setup(x => x.Stop());

            recorderServiceMock.Raise(x => x.Stopped += null, EventArgs.Empty);

            reminderMock.VerifyAll();
        }

        [Test]
        public void Message_is_displayed_when_reminder_rings()
        {
            PauseReminder pauseReminder = CreatePauseReminder();
            pauseReminder.StartMonitoring();
            shellNavigatorMock.Setup(x => x.Navigate(ShellNames.MessageShell, It.IsAny<Dictionary<string, object>>()));

            reminderMock.Raise(x => x.Ring += null, new RingEventArgs());

            shellNavigatorMock.VerifyAll();
        }

        #region PauseInterval

        [Test]
        public void PauseInterval_initial_value()
        {
            PauseReminder pauseReminder = CreatePauseReminder();
            Assert.That(pauseReminder.PauseInterval, Is.EqualTo(TimeSpan.FromHours(1)));
        }

        [Test]
        public void PauseInterval_set_value()
        {
            PauseReminder pauseReminder = CreatePauseReminder();
            pauseReminder.PauseInterval = TimeSpan.FromMilliseconds(100);

            Assert.That(pauseReminder.PauseInterval, Is.EqualTo(TimeSpan.FromMilliseconds(100)));
        }

        #endregion

        [Test]
        public void Pause_message_displayed_for_custom_PauseInterval_value()
        {
            PauseReminder pauseReminder = CreatePauseReminder();
            pauseReminder.PauseInterval = TimeSpan.FromMilliseconds(100);
            bool messageDisplayed = false;
            shellNavigatorMock.Setup(x => x.Navigate(ShellNames.MessageShell, It.IsAny<Dictionary<string, object>>()))
                .Callback(() => messageDisplayed = true);

            //recorderServiceMock.Raise(x => x.Started += null, EventArgs.Empty);
            reminderMock.Raise(x => x.Ring += null, new RingEventArgs());

            Thread.Sleep(120);

            if (!messageDisplayed)
                Assert.Fail("The pause message was not displayed");
        }

        [Test]
        public void StartMonitoring_starts_reminder_if_recorder_is_already_started()
        {
            recorderServiceMock.Setup(x => x.State).Returns(RecorderState.Running);
            reminderMock.Setup(x => x.Start(TimeSpan.FromHours(1)));

            CreatePauseReminder();

            recorderServiceMock.VerifyAll();
            reminderMock.VerifyAll();
        }

        [Test]
        public void SnoozeInterval_initial_value()
        {
            PauseReminder pauseReminder = CreatePauseReminder();

            Assert.That(pauseReminder.SnoozeInterval, Is.EqualTo(TimeSpan.FromMinutes(3)));
        }
    }
}
