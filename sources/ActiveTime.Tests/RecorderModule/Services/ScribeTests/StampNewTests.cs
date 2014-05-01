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
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.RecorderModule.Services;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.RecorderModule.Services.ScribeTests
{
    [TestFixture]
    public class StampNewTests
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
        public void calls_timeProvider()
        {
            DateTime now = DateTime.Now;
            timeProviderMock.Setup(x => x.GetDateTime()).Returns(now);

            scribe.StampNew();

            timeProviderMock.VerifyAll();
        }

        [Test]
        public void saves_a_new_record_in_repository()
        {
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
        public void if_called_twice_adds_new_record()
        {
            repositoryMock.Setup(x => x.Add(It.IsAny<TimeRecord>()));
            repositoryMock.Setup(x => x.Add(It.IsAny<TimeRecord>()));

            scribe.StampNew();
            scribe.StampNew();

            repositoryMock.VerifyAll();
        }
    }
}
