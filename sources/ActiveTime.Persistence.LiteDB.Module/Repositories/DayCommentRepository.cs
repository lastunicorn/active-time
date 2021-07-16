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
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;
using LiteDB;

namespace DustInTheWind.ActiveTime.Persistence.LiteDB.Module.Repositories
{
    internal class DayCommentRepository : IDayCommentRepository
    {
        public const string CollectionName = "DayComment";

        private readonly LiteDatabase database;

        public DayCommentRepository(LiteDatabase database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public void Add(DayRecord dayRecord)
        {
            LiteCollection<DayRecord> dayCommentCollection = database.GetCollection<DayRecord>(CollectionName);

            bool exists = dayCommentCollection
                .Find(x =>
                    x.Date == dayRecord.Date &&
                    x.Comment == dayRecord.Comment)
                .Any();

            if (exists)
            {
                string errorMessage = string.Format("Error adding the comment record '{0}' into the database.", dayRecord);
                throw new PersistenceException(errorMessage);
            }

            dayCommentCollection.Insert(dayRecord);
        }

        public void Update(DayRecord dayRecord)
        {
            if (dayRecord == null) throw new ArgumentNullException(nameof(dayRecord));

            if (dayRecord.Id <= 0)
                throw new PersistenceException("The id of the comment record should be a positive integer.");

            LiteCollection<DayRecord> dayCommentCollection = database.GetCollection<DayRecord>(CollectionName);

            bool exists = dayCommentCollection
                .Find(x => x.Id == dayRecord.Id)
                .Any();

            if (!exists)
                throw new PersistenceException("There is no record with the specified id to update.");

            dayCommentCollection.Update(dayRecord);
        }

        public void AddOrUpdate(DayRecord dayRecord)
        {
            LiteCollection<DayRecord> dayCommentCollection = database.GetCollection<DayRecord>(CollectionName);

            DayRecord existingDayRecord = dayCommentCollection
                .Find(x => x.Date == dayRecord.Date)
                .FirstOrDefault();

            if (existingDayRecord == null)
            {
                dayCommentCollection.Insert(dayRecord);
            }
            else
            {
                existingDayRecord.Comment = dayRecord.Comment;
                dayCommentCollection.Update(existingDayRecord);
            }
        }

        public void Delete(DayRecord dayRecord)
        {
            if (dayRecord == null) throw new ArgumentNullException(nameof(dayRecord));

            if (dayRecord.Id <= 0)
                throw new PersistenceException("The id of the comment record should be a positive integer.");

            LiteCollection<DayRecord> dayCommentCollection = database.GetCollection<DayRecord>(CollectionName);

            bool exists = dayCommentCollection
                .Find(x => x.Id == dayRecord.Id)
                .Any();

            if (!exists)
                throw new PersistenceException("There is no record with the specified id to update.");

            dayCommentCollection.Delete(x => x.Id == dayRecord.Id);
        }

        public DayRecord GetById(int id)
        {
            LiteCollection<DayRecord> dayCommentCollection = database.GetCollection<DayRecord>(CollectionName);

            return dayCommentCollection
                .Find(x => x.Id == id)
                .FirstOrDefault();
        }

        public DayRecord GetByDate(DateTime date)
        {
            LiteCollection<DayRecord> dayCommentCollection = database.GetCollection<DayRecord>(CollectionName);

            return dayCommentCollection
                .Find(x => x.Date == date)
                .FirstOrDefault();
        }

        public List<DayRecord> GetByDate(DateTime startDate, DateTime endDate)
        {
            LiteCollection<DayRecord> dayCommentCollection = database.GetCollection<DayRecord>(CollectionName);

            return dayCommentCollection
                .Find(x => x.Date >= startDate && x.Date <= endDate)
                .ToList();
        }

        public IList<DayRecord> GetAll()
        {
            LiteCollection<DayRecord> dayCommentCollection = database.GetCollection<DayRecord>(CollectionName);

            return dayCommentCollection
                .FindAll()
                .ToList();
        }
    }
}
