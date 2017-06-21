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
using LiteDB;

namespace DustInTheWind.ActiveTime.PersistenceModule.LiteDB.Repositories
{
    internal class TimeRecordRepository : ITimeRecordRepository
    {
        private const string ConnectionString = Constants.DatabaseFileName;
        private const string CollectionName = "TimeRecord";

        public void Add(TimeRecord timeRecord)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                LiteCollection<TimeRecord> timeRecords = db.GetCollection<TimeRecord>(CollectionName);
                timeRecords.Insert(timeRecord);
            }
        }

        public void Update(TimeRecord timeRecord)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                LiteCollection<TimeRecord> timeRecords = db.GetCollection<TimeRecord>(CollectionName);
                timeRecords.Update(timeRecord);
            }
        }

        public void Delete(TimeRecord timeRecord)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                LiteCollection<TimeRecord> timeRecords = db.GetCollection<TimeRecord>(CollectionName);
                timeRecords.Delete(x => x.Id == timeRecord.Id);
            }
        }

        public TimeRecord GetById(int id)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                LiteCollection<TimeRecord> timeRecords = db.GetCollection<TimeRecord>(CollectionName);
                return timeRecords.Find(x => x.Id == id)
                    .FirstOrDefault();
            }
        }

        public IList<TimeRecord> GetByDate(DateTime date)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                LiteCollection<TimeRecord> timeRecords = db.GetCollection<TimeRecord>(CollectionName);
                return timeRecords.Find(x => x.Date == date)
                    .ToList();
            }
        }
    }
}
