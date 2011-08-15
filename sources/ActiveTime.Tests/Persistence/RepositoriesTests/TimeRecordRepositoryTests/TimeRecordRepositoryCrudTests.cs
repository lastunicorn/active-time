using System;
using DustInTheWind.ActiveTime.Persistence.Entities;
using DustInTheWind.ActiveTime.Persistence.Repositories;
using NUnit.Framework;
using System.Collections.Generic;

namespace DustInTheWind.ActiveTime.UnitTests.Persistence.RepositoriesTests.TimeRecordRepositoryTests
{
    [TestFixture]
    public class TimeRecordRepositoryCrudTests : RepositoryCrudTestsBase<TimeRecord, TimeRecordRepository>
    {

        protected override TimeRecordRepository CreateRepository()
        {
            return new TimeRecordRepository(CurrentSession);
        }

        private DateTime date = new DateTime(2000, 06, 13);
        int index = 0;

        protected override TimeRecord CreateEntity()
        {
            return new TimeRecord
            {
                Date = date.AddDays(index++),
                StartTime = new TimeSpan(7, 30, 12),
                EndTime = new TimeSpan(10, 13, 46),
                RecordType = TimeRecordType.Normal
            };
        }

        protected override void ModifyAllButBusinessKey(TimeRecord entity)
        {
            entity.RecordType = TimeRecordType.Fake;
        }

        protected override bool AreEquals(TimeRecord a, TimeRecord b)
        {
            return a != null && b != null && a.Id == b.Id && a.Date == b.Date && a.StartTime == b.StartTime && a.EndTime == b.EndTime && a.RecordType == b.RecordType;
        }

        protected override bool HasBusinessKey
        {
            get { return true; }
        }

        protected override bool IsBusinessKeyMutable
        {
            get { return true; }
        }

        protected override void ModifyBusinessKey(TimeRecord entity)
        {
            entity.Date = new DateTime(2011, 06, 13);
            entity.StartTime = new TimeSpan(8, 30, 12);
            entity.EndTime = new TimeSpan(11, 13, 46);
        }

        protected override ICollection<TimeRecord> CreateEntities(int count)
        {
            List<TimeRecord> list = new List<TimeRecord>();

            for (int i = 0; i < count; i++)
            {
                list.Add(CreateEntity());
            }

            return list;
        }
    }
}
