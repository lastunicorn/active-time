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
        private Mock<ITimeProvider> timeProvider;
        private Mock<IUnitOfWorkFactory> unitOfWorkFactory;
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<ITimeRecordRepository> timeRecordRepository;
        private Scribe scribe;

        [SetUp]
        public void SetUp()
        {
            timeProvider = new Mock<ITimeProvider>();
            unitOfWorkFactory = new Mock<IUnitOfWorkFactory>();
            unitOfWork = new Mock<IUnitOfWork>();
            timeRecordRepository = new Mock<ITimeRecordRepository>();

            unitOfWorkFactory
                .Setup(x => x.CreateNew())
                .Returns(unitOfWork.Object);

            unitOfWork
                .Setup(x => x.TimeRecordRepository)
                .Returns(timeRecordRepository.Object);

            scribe = new Scribe(timeProvider.Object, unitOfWorkFactory.Object);
        }

        [Test]
        public void calls_timeProvider()
        {
            DateTime now = DateTime.Now;
            timeProvider.Setup(x => x.GetDateTime()).Returns(now);

            scribe.StampNew();

            timeProvider.VerifyAll();
        }

        [Test]
        public void saves_a_new_record_in_repository()
        {
            DateTime now = DateTime.Now;
            timeProvider.Setup(x => x.GetDateTime()).Returns(now);
            timeRecordRepository.Setup(x => x.Add(It.Is<TimeRecord>(r => r.Id == 0 &&
                                                                   r.RecordType == TimeRecordType.Normal &&
                                                                   r.Date == now.Date &&
                                                                   (r.StartTime - now.TimeOfDay) < TimeSpan.FromSeconds(1) &&
                                                                   r.EndTime == r.StartTime)));
            scribe.StampNew();

            timeRecordRepository.VerifyAll();
        }

        [Test]
        public void if_called_twice_adds_new_record()
        {
            timeRecordRepository.Setup(x => x.Add(It.IsAny<TimeRecord>()));
            timeRecordRepository.Setup(x => x.Add(It.IsAny<TimeRecord>()));

            scribe.StampNew();
            scribe.StampNew();

            timeRecordRepository.VerifyAll();
        }
    }
}
