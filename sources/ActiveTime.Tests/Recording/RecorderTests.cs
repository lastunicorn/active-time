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
    public class RecorderTests
    {
        private Recorder recorder;
        private Mock<ITimeRecordRepository> recordRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            recordRepositoryMock = new Mock<ITimeRecordRepository>();
            recorder = new Recorder(recordRepositoryMock.Object);
        }

        #region Constructor

        [Test]
        public void TestConstructorOk()
        {
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorNull()
        {
            new Recorder(null);
        }

        [Test]
        public void TestInitial_State()
        {
            Assert.That(recorder.State, Is.EqualTo(RecorderState.Stopped));
        }

        [Test]
        public void TestInitial_GetTimeFromLastStop()
        {
            TimeSpan? actualTime = recorder.GetTimeFromLastStop();

            Assert.That(actualTime, Is.Null);
        }

        #endregion

        #region Start

        [Test]
        public void TestStart_SaveNewRecord()
        {
            // Prepare
            TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
            recordRepositoryMock.Setup(x => x.Add(It.Is<TimeRecord>(r => r.Id == 0 && r.Date == DateTime.Today && r.StartTime > timeOfDay - TimeSpan.FromSeconds(1) && r.StartTime < timeOfDay + TimeSpan.FromSeconds(1) && r.RecordType == TimeRecordType.Normal)));

            // Execute
            recorder.Start();

            // Verify
            recordRepositoryMock.VerifyAll();
        }

        [Test]
        public void TestStart_State()
        {
            // Execute
            recorder.Start();

            // Verify
            Assert.That(recorder.State, Is.EqualTo(RecorderState.Running));
        }

        [Test]
        public void TestStart_CallWhenAlreadyStarted()
        {
            // Prepare
            recordRepositoryMock.Setup(x => x.Add(It.IsAny<TimeRecord>()));
            recorder.Start();

            // Execute
            recorder.Start();

            // Verify
            recordRepositoryMock.Verify(x => x.Add(It.IsAny<TimeRecord>()), Times.Once());
            recordRepositoryMock.Verify(x => x.Update(It.IsAny<TimeRecord>()), Times.Never());
            recordRepositoryMock.VerifyAll();
        }

        [Test]
        [Timeout(200)]
        public void TestStart_StartedEventCalled()
        {
            // Prepare
            ManualResetEvent ev = new ManualResetEvent(false);
            recorder.Started += new EventHandler((sender, e) => { ev.Set(); });

            // Execute
            recorder.Start();

            // Verify
            ev.WaitOne();
        }

        [Test]
        [Timeout(200)]
        public void TestStart_StartedEvent_Sender()
        {
            // Prepare
            ManualResetEvent ev = new ManualResetEvent(false);
            object actualSender = null;
            recorder.Started += new EventHandler((sender, e) => { actualSender = sender; ev.Set(); });

            // Execute
            recorder.Start();

            // Verify
            ev.WaitOne();
            Assert.That(actualSender, Is.SameAs(recorder));
        }

        #endregion

        #region Stamp - Initial Stopped

        [Test]
        public void TestStamp_InitialStopped_SaveNewRecord()
        {
            // Prepare
            TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
            recordRepositoryMock.Setup(x => x.Add(It.Is<TimeRecord>(r => r.Id == 0 && r.Date == DateTime.Today && r.StartTime > timeOfDay - TimeSpan.FromSeconds(1) && r.StartTime < timeOfDay + TimeSpan.FromSeconds(1) && r.RecordType == TimeRecordType.Normal)));

            // Execute
            recorder.Stamp();

            // Verify
            recordRepositoryMock.VerifyAll();
        }

        [Test]
        public void TestStamp_InitialStopped_CheckState()
        {
            // Execute
            recorder.Stamp();

            // Verify
            Assert.That(recorder.State, Is.EqualTo(RecorderState.Running));
        }

        [Test]
        [Timeout(200)]
        public void TestStart_InitialStopped_StampingEventCalled()
        {
            // Prepare
            ManualResetEvent ev = new ManualResetEvent(false);
            recorder.Stamping += new EventHandler((sender, e) => { ev.Set(); });

            // Execute
            recorder.Stamp();

            // Verify
            ev.WaitOne();
        }

        [Test]
        [Timeout(200)]
        public void TestStart_InitialStopped_StampingEvent_CheckSender()
        {
            // Prepare
            ManualResetEvent ev = new ManualResetEvent(false);
            object actualSender = null;
            recorder.Stamping += new EventHandler((sender, e) => { actualSender = sender; ev.Set(); });

            // Execute
            recorder.Stamp();

            // Verify
            ev.WaitOne();
            Assert.That(actualSender, Is.SameAs(recorder));
        }

        [Test]
        [Timeout(200)]
        public void TestStamp_InitialStopped_StartedEventCalled()
        {
            // Prepare
            ManualResetEvent ev = new ManualResetEvent(false);
            recorder.Started += new EventHandler((sender, e) => { ev.Set(); });

            // Execute
            recorder.Stamp();

            // Verify
            ev.WaitOne();
        }

        [Test]
        [Timeout(200)]
        public void TestStart_InitialStopped_StartedEvent_CheckSender()
        {
            // Prepare
            ManualResetEvent ev = new ManualResetEvent(false);
            object actualSender = null;
            recorder.Started += new EventHandler((sender, e) => { actualSender = sender; ev.Set(); });

            // Execute
            recorder.Stamp();

            // Verify
            ev.WaitOne();
            Assert.That(actualSender, Is.SameAs(recorder));
        }

        [Test]
        [Timeout(200)]
        public void TestStart_InitialStopped_StampedEventCalled()
        {
            // Prepare
            ManualResetEvent ev = new ManualResetEvent(false);
            recorder.Stamped += new EventHandler((sender, e) => { ev.Set(); });

            // Execute
            recorder.Stamp();

            // Verify
            ev.WaitOne();
        }

        [Test]
        [Timeout(200)]
        public void TestStart_InitialStopped_StampedEvent_CheckSender()
        {
            // Prepare
            ManualResetEvent ev = new ManualResetEvent(false);
            object actualSender = null;
            recorder.Stamped += new EventHandler((sender, e) => { actualSender = sender; ev.Set(); });

            // Execute
            recorder.Stamp();

            // Verify
            ev.WaitOne();
            Assert.That(actualSender, Is.SameAs(recorder));
        }

        #endregion

        #region Stamp - Initial Running

        [Test]
        public void TestStamp_InitialRunning_UpdateRecord()
        {
            // Prepare
            TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
            recordRepositoryMock.Setup(x => x.Update(It.Is<TimeRecord>(r => r.Id == 0 && r.Date == DateTime.Today && r.StartTime > timeOfDay - TimeSpan.FromSeconds(1) && r.StartTime < timeOfDay + TimeSpan.FromSeconds(1) && r.RecordType == TimeRecordType.Normal)));
            recorder.Start();

            // Execute
            recorder.Stamp();

            // Verify
            recordRepositoryMock.VerifyAll();
        }

        [Test]
        public void TestStamp_InitialRunning_CheckState()
        {
            // Prepare
            recorder.Start();

            // Execute
            recorder.Stamp();

            // Verify
            Assert.That(recorder.State, Is.EqualTo(RecorderState.Running));
        }

        [Test]
        [Timeout(200)]
        public void TestStamp_InitialRunning_StampingEventCalled()
        {
            // Prepare
            recorder.Start();
            ManualResetEvent ev = new ManualResetEvent(false);
            recorder.Stamping += new EventHandler((sender, e) => { ev.Set(); });

            // Execute
            recorder.Stamp();

            // Verify
            ev.WaitOne();
        }

        [Test]
        [Timeout(200)]
        public void TestStamp_InitialRunning_StampingEvent_CheckSender()
        {
            // Prepare
            recorder.Start();
            ManualResetEvent ev = new ManualResetEvent(false);
            object actualSender = null;
            recorder.Stamping += new EventHandler((sender, e) => { actualSender = sender; ev.Set(); });

            // Execute
            recorder.Stamp();

            // Verify
            ev.WaitOne();
            Assert.That(actualSender, Is.SameAs(recorder));
        }

        [Test]
        [Timeout(200)]
        public void TestStart_InitialRunning_StampedEventCalled()
        {
            // Prepare
            ManualResetEvent ev = new ManualResetEvent(false);
            recorder.Stamped += new EventHandler((sender, e) => { ev.Set(); });
            recorder.Start();

            // Execute
            recorder.Stamp();

            // Verify
            ev.WaitOne();
        }

        [Test]
        [Timeout(200)]
        public void TestStart_InitialRunning_StampedEvent_CheckSender()
        {
            // Prepare
            ManualResetEvent ev = new ManualResetEvent(false);
            object actualSender = null;
            recorder.Stamped += new EventHandler((sender, e) => { actualSender = sender; ev.Set(); });
            recorder.Start();

            // Execute
            recorder.Stamp();

            // Verify
            ev.WaitOne();
            Assert.That(actualSender, Is.SameAs(recorder));
        }

        #endregion

        #region Stop - Initial Stopped

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
        public void TestStamp_InitialStopped_StoppedEventNotRaised()
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

        #region Stop - Initial Running

        [Test]
        public void TestStop_InitialRunning_UpdateRecord()
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
        public void TestStop_InitialRunning_CheckState()
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

        #region GetTimeFromLastStop

        [Test]
        public void TestStart_GetTimeFromLastStop()
        {
            // Prepare
            recorder.Start();

            // Execute
            TimeSpan? actualTime = recorder.GetTimeFromLastStop();

            // Verify
            Assert.That(actualTime, Is.Null);
        }

        #endregion
    }
}
