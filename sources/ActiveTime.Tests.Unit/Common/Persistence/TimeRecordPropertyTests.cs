// ActiveTime
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
using DustInTheWind.ActiveTime.Common;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Common.Persistence
{
    [TestFixture]
    public class TimeRecordPropertyTests
    {
        private TimeRecord timeRecord;

        [SetUp]
        public void SetUp()
        {
            timeRecord = new TimeRecord();
        }

        [Test]
        public void TestDate_GetSet()
        {
            DateTime date = new DateTime(2011, 06, 13);
            timeRecord.Date = date;

            Assert.That(timeRecord.Date, Is.EqualTo(date));
        }

        [Test]
        public void TestStartTime_GetSet()
        {
            TimeSpan startTime = new TimeSpan(12, 10, 30);
            timeRecord.StartTime = startTime;

            Assert.That(timeRecord.StartTime, Is.EqualTo(startTime));
        }

        [Test]
        public void TestEndTime_GetSet()
        {
            TimeSpan endTime = new TimeSpan(12, 10, 30);
            timeRecord.EndTime = endTime;

            Assert.That(timeRecord.EndTime, Is.EqualTo(endTime));
        }

        [Test]
        public void TestRecordType_GetSet()
        {
            TimeRecordType recordType = TimeRecordType.Fake;
            timeRecord.RecordType = recordType;

            Assert.That(timeRecord.RecordType, Is.EqualTo(recordType));
        }
    }
}
