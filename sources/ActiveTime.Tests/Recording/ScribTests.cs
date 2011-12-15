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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.RecorderModule.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Recording
{
    [TestFixture]
    public class ScribTests
    {
        private Mock<ITimeRecordRepository> repositoryMock;
        private Mock<ITimeProvider> timeProviderMock;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<ITimeRecordRepository>();
            timeProviderMock = new Mock<ITimeProvider>();
        }

        private Scrib CreateRecorder()
        {
            return new Scrib(repositoryMock.Object, timeProviderMock.Object);
        }

        [Test]
        public void Constructor()
        {
            Scrib recorder = CreateRecorder();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_with_null_repository()
        {
            Scrib recorder = new Scrib(null, timeProviderMock.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_with_null_timeProvider()
        {
            Scrib recorder = new Scrib(repositoryMock.Object, null);
        }

        [Test]
        public void StampNew_calls_timeProvider()
        {
            Scrib recorder = CreateRecorder();
            DateTime now = DateTime.Now;
            timeProviderMock.Setup(x => x.GetDateTime()).Returns(now);
            
            recorder.StampNew();

            timeProviderMock.VerifyAll();
        }

        [Test]
        public void StampNew_saves_a_new_record_in_repository()
        {
            Scrib recorder = CreateRecorder();
            DateTime now = DateTime.Now;
            timeProviderMock.Setup(x => x.GetDateTime()).Returns(now);
            repositoryMock.Setup(x => x.Add(It.Is<TimeRecord>(r => r.Id == 0 &&
                                                                   r.RecordType == TimeRecordType.Normal &&
                                                                   r.Date == now.Date &&
                                                                   (r.StartTime - now.TimeOfDay) < TimeSpan.FromSeconds(1) &&
                                                                   r.EndTime == r.StartTime)));
            recorder.StampNew();

            repositoryMock.VerifyAll();
        }

        [Test]
        public void StampNew_called_twice_adds_new_record()
        {
            Scrib recorder = CreateRecorder();

            repositoryMock.Setup(x => x.Add(It.IsAny<TimeRecord>()));
            repositoryMock.Setup(x => x.Add(It.IsAny<TimeRecord>()));

            recorder.StampNew();
            recorder.StampNew();

            repositoryMock.VerifyAll();
        }

        [Test]
        public void Stamp_creates_new_record_if_not_exist()
        {
            Scrib recorder = CreateRecorder();

            repositoryMock.Setup(x => x.Add(It.IsAny<TimeRecord>()));
            repositoryMock.Setup(x => x.Update(It.IsAny<TimeRecord>()));

            recorder.Stamp();

            repositoryMock.VerifyAll();
        }
    }
}
