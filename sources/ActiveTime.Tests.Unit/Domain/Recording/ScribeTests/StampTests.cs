// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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

using DustInTheWind.ActiveTime.Application.Recording;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.DataAccess;
using DustInTheWind.ActiveTime.Ports.SystemAccess;
using Moq;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.Tests.Unit.Domain.Recording.ScribeTests;

[TestFixture]
public class StampTests
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

        CurrentDay currentDay = new();

        scribe = new Scribe(systemClock.Object, unitOfWork.Object, currentDay);
    }

    [Test]
    public void saves_new_record_in_persistence_if_stamping_for_the_first_time()
    {
        TimeRecord actualTimeRecord = null;
        DateTime currentTime = new(1980, 05, 13, 12, 59, 00);
        systemClock
            .Setup(x => x.GetCurrentTime())
            .Returns(currentTime);
        timeRecordRepository
            .Setup(x => x.Add(It.IsAny<TimeRecord>()))
            .Callback<TimeRecord>(timeRecord => actualTimeRecord = timeRecord);

        scribe.Stamp();

        timeRecordRepository.VerifyAll();
        Assert.That(actualTimeRecord.RecordType, Is.EqualTo(TimeRecordType.Normal));
        Assert.That(actualTimeRecord.Date, Is.EqualTo(currentTime.Date));
        Assert.That(actualTimeRecord.StartTime, Is.EqualTo(currentTime.TimeOfDay));
        Assert.That(actualTimeRecord.EndTime, Is.EqualTo(currentTime.TimeOfDay));
    }

    [Test]
    public void creates_new_record_in_persistence_if_second_stamp_is_in_next_day()
    {
        DateTime firstDateTime = new(1980, 06, 13, 10, 13, 50);
        StampNew(firstDateTime);

        TimeRecord actualTimeRecord = null;
        timeRecordRepository
            .Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(new TimeRecord
            {
                RecordType = TimeRecordType.Normal,
                Date = new DateTime(1980, 06, 13),
                StartTime = new TimeSpan(10, 13, 50),
                EndTime = new TimeSpan(10, 15, 26)
            });
        timeRecordRepository
            .Setup(x => x.Add(It.IsAny<TimeRecord>()))
            .Callback<TimeRecord>(x => actualTimeRecord = x);

        DateTime secondDateTime = new(1980, 06, 14, 02, 30, 10);
        systemClock
            .Setup(x => x.GetCurrentTime())
            .Returns(secondDateTime);

        scribe.Stamp();

        Assert.That(actualTimeRecord.Date, Is.EqualTo(secondDateTime.Date));
        Assert.That(actualTimeRecord.StartTime, Is.EqualTo(TimeSpan.Zero));
        Assert.That(actualTimeRecord.EndTime, Is.EqualTo(secondDateTime.TimeOfDay));
    }

    // todo: What happens if second stamp is after two days?

    private void StampNew(DateTime dateTime)
    {
        systemClock
            .Setup(x => x.GetCurrentTime())
            .Returns(dateTime);

        scribe.StampNew();
    }
}