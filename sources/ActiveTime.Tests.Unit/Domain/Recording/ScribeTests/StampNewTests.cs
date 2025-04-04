﻿// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
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
using System.Collections.Generic;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Ports.SystemAccess;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Domain.Recording.ScribeTests
{
    [TestFixture]
    public class StampNewTests
    {
        private Mock<ISystemClock> systemClock;
        private Mock<IUnitOfWork> unitOfWork;
        private Mock<ITimeRecordRepository> timeRecordRepository;
        private Scribe scribe;

        [SetUp]
        public void SetUp()
        {
            systemClock = new Mock<ISystemClock>();
            unitOfWork = new Mock<IUnitOfWork>();
            timeRecordRepository = new Mock<ITimeRecordRepository>();

            unitOfWork
                .Setup(x => x.TimeRecordRepository)
                .Returns(timeRecordRepository.Object);

            InMemoryState inMemoryState = new InMemoryState();

            scribe = new Scribe(systemClock.Object, unitOfWork.Object, inMemoryState);
        }

        [Test]
        public void calls_timeProvider()
        {
            DateTime now = DateTime.Now;
            systemClock.Setup(x => x.GetCurrentTime()).Returns(now);

            scribe.StampNew();

            systemClock.VerifyAll();
        }

        [Test]
        public void saves_a_new_record_in_repository()
        {
            TimeRecord savedTimeRecord = null;
            DateTime now = new DateTime(2021, 09, 06, 12, 44, 22);
            systemClock
                .Setup(x => x.GetCurrentTime())
                .Returns(now);
            timeRecordRepository
                .Setup(x => x.Add(It.IsAny<TimeRecord>()))
                .Callback<TimeRecord>(x =>
                {
                    savedTimeRecord = x;
                });

            //timeRecordRepository
            //    .Setup(x => x.Add(It.Is<TimeRecord>(r => r.Id == 0 &&
            //                                                       r.RecordType == TimeRecordType.Normal &&
            //                                                       r.Date == now.Date &&
            //                                                       (r.StartTime - now.TimeOfDay) < TimeSpan.FromSeconds(1) &&
            //                                                       r.EndTime == r.StartTime)));

            scribe.StampNew();

            timeRecordRepository.VerifyAll();
            Assert.AreEqual(0, savedTimeRecord.Id);
            Assert.AreEqual(TimeRecordType.Normal, savedTimeRecord.RecordType);
            Assert.AreEqual(now.Date, savedTimeRecord.Date);
            Assert.AreEqual(now.TimeOfDay, savedTimeRecord.StartTime);
            Assert.AreEqual(now.TimeOfDay, savedTimeRecord.EndTime);
        }

        [Test]
        public void if_called_twice_adds_new_record()
        {
            List<TimeRecord> timeRecords = new List<TimeRecord>();
            timeRecordRepository
                .Setup(x => x.Add(It.IsAny<TimeRecord>()))
                .Callback<TimeRecord>(x =>
                {
                    timeRecords.Add(x);
                });

            scribe.StampNew();
            scribe.StampNew();

            timeRecordRepository.VerifyAll();
            Assert.AreEqual(2, timeRecords.Count);
            Assert.AreNotSame(timeRecords[0], timeRecords[1]);
        }
    }
}
