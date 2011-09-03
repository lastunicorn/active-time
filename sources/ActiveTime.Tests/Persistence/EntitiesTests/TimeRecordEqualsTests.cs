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
using DustInTheWind.ActiveTime.Persistence.Entities;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.EntitiesTests
{
    [TestFixture]
    public class TimeRecordEqualsTests
    {
        #region Equals

        [Test]
        public void TestEqualsOk()
        {
            TimeRecord record1 = BuildNewTimeRecord();
            TimeRecord record2 = BuildNewTimeRecord();

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_SameInstance()
        {
            TimeRecord record1 = BuildNewTimeRecord();
            TimeRecord record2 = BuildNewTimeRecord();

            bool actualValue = record1.Equals(record1);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_DifferentId()
        {
            TimeRecord record1 = BuildNewTimeRecord();
            TimeRecord record2 = BuildNewTimeRecord();
            record2.Id = 10;

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_DifferentDate()
        {
            TimeRecord record1 = BuildNewTimeRecord();
            TimeRecord record2 = BuildNewTimeRecord();
            record2.Date = new DateTime(2011, 03, 05);

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.False);
        }

        [Test]
        public void TestEquals_DifferentRecordType()
        {
            TimeRecord record1 = BuildNewTimeRecord();
            TimeRecord record2 = BuildNewTimeRecord();
            record2.RecordType = TimeRecordType.Fake;

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.True);
        }

        [Test]
        public void TestEquals_DifferentStartTime()
        {
            TimeRecord record1 = BuildNewTimeRecord();
            TimeRecord record2 = BuildNewTimeRecord();
            record2.StartTime = TimeSpan.FromHours(20);

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.False);
        }

        [Test]
        public void TestEquals_DifferentEndTime()
        {
            TimeRecord record1 = BuildNewTimeRecord();
            TimeRecord record2 = BuildNewTimeRecord();
            record2.EndTime = TimeSpan.FromHours(20);

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.False);
        }

        [Test]
        public void TestEquals_AllDifferent()
        {
            TimeRecord record1 = BuildNewTimeRecord();
            TimeRecord record2 = BuildNewTimeRecord();
            record2.Id = 10;
            record2.Date = new DateTime(2011, 03, 05);
            record2.RecordType = TimeRecordType.Fake;
            record2.StartTime = TimeSpan.FromHours(20);
            record2.EndTime = TimeSpan.FromHours(21);

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.False);
        }

        [Test]
        public void TestEquals_AllDifferentButBusinessKey()
        {
            TimeRecord record1 = BuildNewTimeRecord();
            TimeRecord record2 = BuildNewTimeRecord();
            record2.Id = 10;
            record2.RecordType = TimeRecordType.Fake;

            bool actualValue = record1.Equals(record2);

            Assert.That(actualValue, Is.True);
        }

        #endregion

        #region Utils

        private TimeRecord BuildNewTimeRecord()
        {
            return new TimeRecord
            {
                Id = 5,
                Date = new DateTime(2000, 06, 13),
                RecordType = TimeRecordType.Normal,
                StartTime = new TimeSpan(1, 30, 20),
                EndTime = new TimeSpan(12, 15, 30)
            };
        }

        #endregion
    }
}
