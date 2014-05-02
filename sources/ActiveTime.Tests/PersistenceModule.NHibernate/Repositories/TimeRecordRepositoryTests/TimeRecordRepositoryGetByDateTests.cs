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
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.PersistenceModule.NHibernate.Repositories;
using NUnit.Framework;

namespace DustInTheWind.ActiveTime.UnitTests.PersistenceModule.NHibernate.Repositories.TimeRecordRepositoryTests
{
    [TestFixture]
    public class TimeRecordRepositoryGetByDateTests : RepositoryTestsBase
    {
        private TimeRecordRepository timeRecordRepository;
        private IList<TimeRecord> timeRecords1;
        private IList<TimeRecord> timeRecords2;
        private IList<TimeRecord> timeRecords3;

        //protected TimeRecord CreateTimeRecord(DateTime date)
        //{
        //    return new TimeRecord
        //    {
        //        Date = date,
        //        StartTime = new TimeSpan(7, 30, 12),
        //        EndTime = new TimeSpan(10, 13, 46),
        //        RecordType = TimeRecordType.Normal
        //    };
        //}

        protected override void OnSetUp()
        {
            base.OnSetUp();

            timeRecordRepository = new TimeRecordRepository(CurrentSession);

            // Multiple records per date
            timeRecords1 = CreateTimeRecords(new DateTime(2000, 06, 13), 5, true);
            foreach (TimeRecord timeRecord in timeRecords1)
            {
                CurrentSession.Save(timeRecord);
                CurrentSession.Flush();
                CurrentSession.Evict(timeRecord);
            }

            // One record per date.
            timeRecords2 = CreateTimeRecords(new DateTime(2011, 06, 13), 1, true);
            foreach (TimeRecord timeRecord in timeRecords2)
            {
                CurrentSession.Save(timeRecord);
                CurrentSession.Flush();
                CurrentSession.Evict(timeRecord);
            }

            // Multiple records per date reversed
            timeRecords3 = CreateTimeRecords(new DateTime(1980, 06, 13), 5, false);
            foreach (TimeRecord timeRecord in timeRecords3)
            {
                CurrentSession.Save(timeRecord);
                CurrentSession.Flush();
                CurrentSession.Evict(timeRecord);
            }
        }

        private IList<TimeRecord> CreateTimeRecords(DateTime date, int count, bool ascending)
        {
            List<TimeRecord> list = new List<TimeRecord>();

            TimeSpan timeofday = new TimeSpan(7, 0, 0);

            for (int i = 0; i < count; i++)
            {
                if (ascending)
                {
                    list.Add(new TimeRecord
                    {
                        Date = date,
                        StartTime = timeofday.Add(TimeSpan.FromMinutes(i * 20)),
                        EndTime = timeofday.Add(TimeSpan.FromMinutes(i * 20 + 10)),
                        RecordType = TimeRecordType.Normal
                    });
                }
                else
                {
                    list.Add(new TimeRecord
                    {
                        Date = date,
                        StartTime = timeofday.Subtract(TimeSpan.FromMinutes(i * 20 + 20)),
                        EndTime = timeofday.Subtract(TimeSpan.FromMinutes(i * 20 + 10)),
                        RecordType = TimeRecordType.Normal
                    });
                }
            }

            return list;
        }

        private class TimeRecordEqualityComparer : IEqualityComparer<TimeRecord>
        {
            public bool Equals(TimeRecord x, TimeRecord y)
            {
                return TimeRecordAssertUtil.AreEquals(x, y);
            }

            public int GetHashCode(TimeRecord obj)
            {
                return obj.GetHashCode();
            }
        }

        private class TimeRecordStartTimeComparer : IComparer<TimeRecord>
        {
            public int Compare(TimeRecord x, TimeRecord y)
            {
                if (x == null && y == null) return 0;
                if (x == null) return -1;
                if (y == null) return 1;

                return x.StartTime.CompareTo(y.StartTime);
            }
        }

        [Test]
        public void TestGetByDate_First()
        {
            IList<TimeRecord> actualTimeRecords = timeRecordRepository.GetByDate(new DateTime(2000, 06, 13));

            TimeRecord[] ordered = timeRecords1.ToArray();
            Array.Sort(ordered, new TimeRecordStartTimeComparer());

            Assert.That(actualTimeRecords, Is.EqualTo(ordered).Using(new TimeRecordEqualityComparer()));
        }

        [Test]
        public void TestGetByDate_Second()
        {
            IList<TimeRecord> actualTimeRecords = timeRecordRepository.GetByDate(new DateTime(2011, 06, 13));

            Assert.That(actualTimeRecords.Count, Is.EqualTo(1));
            Assert.That(actualTimeRecords[0], Is.EqualTo(timeRecords2[0]).Using(new TimeRecordEqualityComparer()));
        }

        [Test]
        public void TestGetByDate_Third()
        {
            IList<TimeRecord> actualTimeRecords = timeRecordRepository.GetByDate(new DateTime(1980, 06, 13));

            TimeRecord[] ordered = timeRecords3.ToArray();
            Array.Sort(ordered, new TimeRecordStartTimeComparer());

            Assert.That(actualTimeRecords, Is.EqualTo(ordered).Using(new TimeRecordEqualityComparer()));
        }

        [Test]
        public void TestGetByDate_Inexistent()
        {
            IList<TimeRecord> actualTimeRecords = timeRecordRepository.GetByDate(new DateTime(1900, 04, 01));

            Assert.That(actualTimeRecords, Is.Empty);
        }
    }
}
