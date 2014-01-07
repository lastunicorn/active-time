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

        private Scribe CreateScrib()
        {
            return new Scribe(repositoryMock.Object, timeProviderMock.Object);
        }

        #region Constructor Tests

        [Test]
        public void Constructor()
        {
            CreateScrib();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_with_null_repository()
        {
            new Scribe(null, timeProviderMock.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_with_null_timeProvider()
        {
            new Scribe(repositoryMock.Object, null);
        }

        #endregion

        [Test]
        public void StampNew_calls_timeProvider()
        {
            Scribe scribe = CreateScrib();
            DateTime now = DateTime.Now;
            timeProviderMock.Setup(x => x.GetDateTime()).Returns(now);

            scribe.StampNew();

            timeProviderMock.VerifyAll();
        }

        [Test]
        public void StampNew_saves_a_new_record_in_repository()
        {
            Scribe scribe = CreateScrib();
            DateTime now = DateTime.Now;
            timeProviderMock.Setup(x => x.GetDateTime()).Returns(now);
            repositoryMock.Setup(x => x.Add(It.Is<TimeRecord>(r => r.Id == 0 &&
                                                                   r.RecordType == TimeRecordType.Normal &&
                                                                   r.Date == now.Date &&
                                                                   (r.StartTime - now.TimeOfDay) < TimeSpan.FromSeconds(1) &&
                                                                   r.EndTime == r.StartTime)));
            scribe.StampNew();

            repositoryMock.VerifyAll();
        }

        [Test]
        public void StampNew_called_twice_adds_new_record()
        {
            Scribe scribe = CreateScrib();

            repositoryMock.Setup(x => x.Add(It.IsAny<TimeRecord>()));
            repositoryMock.Setup(x => x.Add(It.IsAny<TimeRecord>()));

            scribe.StampNew();
            scribe.StampNew();

            repositoryMock.VerifyAll();
        }

        [Test]
        public void Stamp_creates_new_record_if_not_exist()
        {
            Scribe scribe = CreateScrib();

            repositoryMock.Setup(x => x.Add(It.IsAny<TimeRecord>()));

            scribe.Stamp();

            repositoryMock.VerifyAll();
        }

        [Test]
        public void Stamp_updates_an_existing_record()
        {
            DateTime firstDateTime = new DateTime(1980, 06, 13, 10, 13, 50);
            DateTime secondDateTime = new DateTime(1980, 06, 13, 12, 30, 10);
            TimeRecord record = null;

            Scribe scribe = CreateScrib();
            timeProviderMock.Setup(x => x.GetDateTime()).Returns(firstDateTime);
            scribe.StampNew();
            repositoryMock.Setup(x => x.Update(It.IsAny<TimeRecord>())).Callback((TimeRecord a) => record = a);
            timeProviderMock.Setup(x => x.GetDateTime()).Returns(secondDateTime);

            scribe.Stamp();

            Assert.That(record.Date, Is.EqualTo(firstDateTime.Date));
            Assert.That(record.StartTime, Is.EqualTo(new TimeSpan(10, 13, 50)));
            Assert.That(record.EndTime, Is.EqualTo(new TimeSpan(12, 30, 10)));
            repositoryMock.VerifyAll();
        }
    }
}
