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
using DustInTheWind.ActiveTime.Persistence.Repositories;
using DustInTheWind.ActiveTime.Recording;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Recording
{
    [TestFixture]
    public class RecorderInitialTests
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
        public void TestConstructorOk()
        {
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorNull()
        {
            new Recorder2(null);
        }

        //[Test]
        //public void TestInitial_CurrentRecord()
        //{
        //    Assert.That(recorder.CurrentRecord, Is.Null);
        //}

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
    }
}
