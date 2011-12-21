using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.ShellNavigation;
using DustInTheWind.ActiveTime.ReminderModule.Services;
using Moq;
using NUnit.Framework;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Reminding;

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

        [Test]
        public void Constructor_run_ok()
        {
            CreatePauseReminder();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_throws_if_recorderService_is_null()
        {
            Mock<IShellNavigator> shellNavigatorMock = new Mock<IShellNavigator>();
            Mock<IReminder> reminderMock = new Mock<IReminder>();

            new PauseReminder(null, shellNavigatorMock.Object, reminderMock.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_throws_if_shellNavigator_is_null()
        {
            Mock<IRecorderService> recordServiceMock = new Mock<IRecorderService>();
            Mock<IReminder> reminderMock = new Mock<IReminder>();

            new PauseReminder(recordServiceMock.Object, null, reminderMock.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_throws_if_reminder_is_null()
        {
            Mock<IRecorderService> recorderServiceMock = new Mock<IRecorderService>();
            Mock<IShellNavigator> shellNavigatorMock = new Mock<IShellNavigator>();

            new PauseReminder(recorderServiceMock.Object, shellNavigatorMock.Object, null);
        }

        [Test]
        public void Reminder_is_started_when_Recorder_starts()
        {
            PauseReminder pauseReminder = CreatePauseReminder();
            reminderMock.Setup(x => x.Start(It.IsAny<TimeSpan>()));

            recorderServiceMock.Raise(x => x.Started += null, EventArgs.Empty);

            reminderMock.VerifyAll();
        }

        [Test]
        public void Reminder_is_stopped_when_Recorder_stops()
        {
            PauseReminder pauseReminder = CreatePauseReminder();
            reminderMock.Setup(x => x.Stop());

            recorderServiceMock.Raise(x => x.Stopped += null, EventArgs.Empty);

            reminderMock.VerifyAll();
        }

        [Test]
        public void Message_is_displayed_when_reminder_rings()
        {
            PauseReminder pauseReminder = CreatePauseReminder();
            shellNavigatorMock.Setup(x => x.Navigate(ShellNames.MessageShell, It.IsAny<Dictionary<string, object>>()));

            reminderMock.Raise(x => x.Ring += null, new RingEventArgs());

            shellNavigatorMock.VerifyAll();
        }
    }
}
