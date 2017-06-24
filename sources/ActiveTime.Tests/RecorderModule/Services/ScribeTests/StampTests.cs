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
    public class StampTests
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
        public void saves_new_record_in_persistence_if_stamping_for_the_first_time()
        {
            TimeRecord actualTimeRecord = null;
            DateTime currentTime = new DateTime(1980, 05, 13, 12, 59, 00);
            timeProvider.Setup(x => x.GetDateTime())
                .Returns(currentTime);
            timeRecordRepository.Setup(x => x.Add(It.IsAny<TimeRecord>()))
                .Callback<TimeRecord>(timeRecord => actualTimeRecord = timeRecord);

            scribe.Stamp();

            timeRecordRepository.VerifyAll();
            Assert.That(actualTimeRecord.RecordType, Is.EqualTo(TimeRecordType.Normal));
            Assert.That(actualTimeRecord.Date, Is.EqualTo(currentTime.Date));
            Assert.That(actualTimeRecord.StartTime, Is.EqualTo(currentTime.TimeOfDay));
            Assert.That(actualTimeRecord.EndTime, Is.EqualTo(currentTime.TimeOfDay));
        }

        [Test]
        public void updates_an_existing_record_in_pesistence_if_second_stemp_in_same_day()
        {
            DateTime firstDateTime = new DateTime(1980, 06, 13, 10, 13, 50);
            DateTime secondDateTime = new DateTime(1980, 06, 13, 12, 30, 10);
            TimeRecord actualTimeRecord = null;
            StampNew(firstDateTime);
            timeProvider.Setup(x => x.GetDateTime())
                .Returns(secondDateTime);
            timeRecordRepository.Setup(x => x.Update(It.IsAny<TimeRecord>()))
                .Callback<TimeRecord>(a => actualTimeRecord = a);

            scribe.Stamp();

            timeRecordRepository.VerifyAll();
            Assert.That(actualTimeRecord.Date, Is.EqualTo(firstDateTime.Date));
            Assert.That(actualTimeRecord.StartTime, Is.EqualTo(firstDateTime.TimeOfDay));
            Assert.That(actualTimeRecord.EndTime, Is.EqualTo(secondDateTime.TimeOfDay));
        }

        [Test]
        public void saves_current_record_in_persistence_if_second_stamp_is_in_next_day()
        {
            DateTime firstDateTime = new DateTime(1980, 06, 13, 10, 13, 50);
            DateTime secondDateTime = new DateTime(1980, 06, 14, 02, 30, 10);
            TimeRecord actualTimeRecord = null;
            StampNew(firstDateTime);
            timeProvider.Setup(x => x.GetDateTime())
                .Returns(secondDateTime);
            int callsCount = 0;
            timeRecordRepository.Setup(x => x.Update(It.IsAny<TimeRecord>()))
                .Callback<TimeRecord>(timeRecord =>
                {
                    callsCount++;
                    if (callsCount == 1)
                        actualTimeRecord = timeRecord;
                });

            scribe.Stamp();

            timeRecordRepository.VerifyAll();
            Assert.That(actualTimeRecord.Date, Is.EqualTo(firstDateTime.Date));
            Assert.That(actualTimeRecord.StartTime, Is.EqualTo(firstDateTime.TimeOfDay));
            Assert.That(actualTimeRecord.EndTime, Is.EqualTo(new TimeSpan(0, 23, 59, 59, 999)));
        }

        [Test]
        public void creates_new_record_in_persistence_if_second_stamp_is_in_next_day()
        {
            DateTime firstDateTime = new DateTime(1980, 06, 13, 10, 13, 50);
            DateTime secondDateTime = new DateTime(1980, 06, 14, 02, 30, 10);
            TimeRecord actualTimeRecord = null;
            StampNew(firstDateTime);
            timeProvider.Setup(x => x.GetDateTime())
                .Returns(secondDateTime);
            timeRecordRepository.Setup(x => x.Add(It.IsAny<TimeRecord>()))
                .Callback<TimeRecord>(timeRecord => actualTimeRecord = timeRecord);

            scribe.Stamp();

            timeRecordRepository.VerifyAll();
            Assert.That(actualTimeRecord.Date, Is.EqualTo(secondDateTime.Date));
            Assert.That(actualTimeRecord.StartTime, Is.EqualTo(TimeSpan.Zero));
            Assert.That(actualTimeRecord.EndTime, Is.EqualTo(secondDateTime.TimeOfDay));
        }

        // todo: What happens if second stamp is after two days?

        private void StampNew(DateTime dateTime)
        {
            timeProvider.Setup(x => x.GetDateTime())
                .Returns(dateTime);

            scribe.StampNew();
        }
    }
}
