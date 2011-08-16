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
using System.Threading;
using DustInTheWind.ActiveTime.Persistence.Entities;
using DustInTheWind.ActiveTime.Persistence.Repositories;
using DustInTheWind.ActiveTime.Recording;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Recording
{
    [TestFixture]
    public class RecorderStopTests
    {
        private Recorder2 recorder;
        private Mock<ITimeRecordRepository> recordRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            recordRepositoryMock = new Mock<ITimeRecordRepository>();
            recorder = new Recorder2(recordRepositoryMock.Object);
        }

        #region Initial Stopped

        [Test]
        public void TestStop_InitialStopped_NoAddCalled()
        {
            // Execute
            recorder.Stop();

            // Verify
            recordRepositoryMock.Verify(x => x.Add(It.IsAny<TimeRecord>()), Times.Never());
        }

        [Test]
        public void TestStop_InitialStopped_NoUpdateCalled()
        {
            // Execute
            recorder.Stop();

            // Verify
            recordRepositoryMock.Verify(x => x.Update(It.IsAny<TimeRecord>()), Times.Never());
        }

        [Test]
        public void TestStop_InitialStopped_CheckState()
        {
            // Execute
            recorder.Stop();

            // Verify
            Assert.That(recorder.State, Is.EqualTo(RecorderState.Stopped));
        }

        [Test]
        public void TestStamp_InitialStopped_NoEventRaised()
        {
            // Prepare
            ManualResetEvent ev = new ManualResetEvent(false);
            recorder.Stopped += new EventHandler((sender, e) => { ev.Set(); });

            // Execute
            recorder.Stop();

            // Verify
            if (ev.WaitOne(200))
            {
                Assert.Fail("Stopped event was raised.");
            }
        }

        #endregion

        #region Initial Running

        [Test]
        public void TestStamp_InitialRunning_UpdateRecord()
        {
            // Prepare
            recorder.Start();

            // Execute
            recorder.Stop();

            // Verify
            TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
            recordRepositoryMock.Verify(x => x.Update(It.Is<TimeRecord>(r => r.Id == 0 && r.Date == DateTime.Today && r.StartTime > timeOfDay - TimeSpan.FromSeconds(1) && r.StartTime < timeOfDay + TimeSpan.FromSeconds(1) && r.RecordType == TimeRecordType.Normal)), Times.Once());
        }

        [Test]
        public void TestStamp_InitialRunning_CheckState()
        {
            // Prepare
            recorder.Start();

            // Execute
            recorder.Stop();

            // Verify
            Assert.That(recorder.State, Is.EqualTo(RecorderState.Stopped));
        }

        [Test]
        [Timeout(200)]
        public void TestStart_InitialRunning_StoppedEventCalled()
        {
            // Prepare
            ManualResetEvent ev = new ManualResetEvent(false);
            recorder.Stopped += new EventHandler((sender, e) => { ev.Set(); });
            recorder.Start();

            // Execute
            recorder.Stop();

            // Verify
            ev.WaitOne();
        }

        [Test]
        [Timeout(200)]
        public void TestStart_InitialRunning_StoppedEvent_CheckSender()
        {
            // Prepare
            ManualResetEvent ev = new ManualResetEvent(false);
            object actualSender = null;
            recorder.Stopped += new EventHandler((sender, e) => { actualSender = sender; ev.Set(); });
            recorder.Start();

            // Execute
            recorder.Stop();

            // Verify
            ev.WaitOne();
            Assert.That(actualSender, Is.SameAs(recorder));
        }

        #endregion
    }
}
