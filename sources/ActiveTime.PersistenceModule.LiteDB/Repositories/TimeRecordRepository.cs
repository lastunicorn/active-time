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
        private const string CollectionName = "TimeRecord";

        private readonly IUnitOfWork unitOfWork;

        public TimeRecordRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));

            this.unitOfWork = unitOfWork;
        }

        public void Add(TimeRecord timeRecord)
        {
            LiteCollection<TimeRecord> timeRecords = unitOfWork.Connection.GetCollection<TimeRecord>(CollectionName);

            bool exists = timeRecords
                .Find(x =>
                    x.Date == timeRecord.Date &&
                    x.StartTime == timeRecord.StartTime &&
                    x.EndTime == timeRecord.EndTime &&
                    x.RecordType == timeRecord.RecordType)
                .Any();

            if (exists)
            {
                string errorMessage = string.Format("Error adding the time record '{0}' into the database.", timeRecord);
                throw new PersistenceException(errorMessage);
            }

            timeRecords.Insert(timeRecord);
        }

        public void Update(TimeRecord timeRecord)
        {
            if (timeRecord == null) throw new ArgumentNullException(nameof(timeRecord));

            if (timeRecord.Id <= 0)
                throw new PersistenceException("The id of the time record should be a positive integer.");

            LiteCollection<TimeRecord> timeRecords = unitOfWork.Connection.GetCollection<TimeRecord>(CollectionName);

            bool exists = timeRecords
                .Find(x => x.Id == timeRecord.Id)
                .Any();

            if (!exists)
                throw new PersistenceException("There is no record with the specified id to update.");

            timeRecords.Update(timeRecord);
        }

        public void Delete(TimeRecord timeRecord)
        {
            if (timeRecord == null) throw new ArgumentNullException(nameof(timeRecord));

            if (timeRecord.Id <= 0)
                throw new PersistenceException("The id of the time record should be a positive integer.");

            LiteCollection<TimeRecord> timeRecords = unitOfWork.Connection.GetCollection<TimeRecord>(CollectionName);

            bool exists = timeRecords
                .Find(x => x.Id == timeRecord.Id)
                .Any();

            if (!exists)
                throw new PersistenceException("There is no record with the specified id to update.");

            timeRecords.Delete(x => x.Id == timeRecord.Id);
        }

        public TimeRecord GetById(int id)
        {
            LiteCollection<TimeRecord> timeRecords = unitOfWork.Connection.GetCollection<TimeRecord>(CollectionName);
            return timeRecords
                .Find(x => x.Id == id)
                .FirstOrDefault();
        }

        public IList<TimeRecord> GetByDate(DateTime date)
        {
            LiteCollection<TimeRecord> timeRecords = unitOfWork.Connection.GetCollection<TimeRecord>(CollectionName);
            return timeRecords
                .Find(x => x.Date == date)
                .ToList();
        }
    }
}
