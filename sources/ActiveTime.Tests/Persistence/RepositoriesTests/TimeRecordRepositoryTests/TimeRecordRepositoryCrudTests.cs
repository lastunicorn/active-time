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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.PersistenceModule.Repositories;
using NUnit.Framework;

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
