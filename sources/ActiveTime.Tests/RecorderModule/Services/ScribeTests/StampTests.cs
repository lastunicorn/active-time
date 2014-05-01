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

namespace DustInTheWind.ActiveTime.UnitTests.RecorderModule.Services.ScribeTests
{
    [TestFixture]
    public class StampTests
    {
        private Mock<ITimeRecordRepository> repositoryMock;
        private Mock<ITimeProvider> timeProviderMock;
        private Scribe scribe;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<ITimeRecordRepository>();
            timeProviderMock = new Mock<ITimeProvider>();

            scribe = new Scribe(repositoryMock.Object, timeProviderMock.Object);
        }

        [Test]
        public void creates_new_record_if_not_exist()
        {
            repositoryMock.Setup(x => x.Add(It.IsAny<TimeRecord>()));

            scribe.Stamp();

            repositoryMock.VerifyAll();
        }

        [Test]
        public void updates_an_existing_record()
        {
            DateTime firstDateTime = new DateTime(1980, 06, 13, 10, 13, 50);
            DateTime secondDateTime = new DateTime(1980, 06, 13, 12, 30, 10);
            TimeRecord record = null;

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
