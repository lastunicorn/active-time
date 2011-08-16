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
    public class RecorderStartTests
    {
        private Recorder2 recorder;
        private Mock<ITimeRecordRepository> recordRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            recordRepositoryMock = new Mock<ITimeRecordRepository>();
            recorder = new Recorder2(recordRepositoryMock.Object);
        }

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
        public void TestStart_GetTimeFromLastStop()
        {
            // Prepare
            recorder.Start();

            // Execute
            TimeSpan? actualTime = recorder.GetTimeFromLastStop();

            // Verify
            Assert.That(actualTime, Is.Null);
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
    }
}
