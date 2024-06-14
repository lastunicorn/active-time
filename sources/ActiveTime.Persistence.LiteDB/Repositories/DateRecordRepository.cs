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
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.Persistence;
using LiteDB;

namespace DustInTheWind.ActiveTime.Persistence.LiteDB.Repositories
{
    internal class DateRecordRepository : IDateRecordRepository
    {
        public const string CollectionName = "DayComment";

        private readonly LiteDatabase database;

        public DateRecordRepository(LiteDatabase database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public void Add(DateRecord dateRecord)
        {
            ILiteCollection<DateRecord> dayCommentCollection = database.GetCollection<DateRecord>(CollectionName);

            bool exists = dayCommentCollection
                .Find(x =>
                    x.Date == dateRecord.Date &&
                    x.Comment == dateRecord.Comment)
                .Any();

            if (exists)
            {
                string errorMessage = string.Format("Error adding the comment record '{0}' into the database.", dateRecord);
                throw new PersistenceException(errorMessage);
            }

            dayCommentCollection.Insert(dateRecord);
        }

        public void Update(DateRecord dateRecord)
        {
            if (dateRecord == null) throw new ArgumentNullException(nameof(dateRecord));

            if (dateRecord.Id <= 0)
                throw new PersistenceException("The id of the comment record should be a positive integer.");

            ILiteCollection<DateRecord> dayCommentCollection = database.GetCollection<DateRecord>(CollectionName);

            bool exists = dayCommentCollection
                .Find(x => x.Id == dateRecord.Id)
                .Any();

            if (!exists)
                throw new PersistenceException("There is no record with the specified id to update.");

            dayCommentCollection.Update(dateRecord);
        }

        public void AddOrUpdate(DateRecord dateRecord)
        {
            ILiteCollection<DateRecord> dayCommentCollection = database.GetCollection<DateRecord>(CollectionName);

            DateRecord existingDateRecord = dayCommentCollection
                .Find(x => x.Date == dateRecord.Date)
                .FirstOrDefault();

            if (existingDateRecord == null)
            {
                dayCommentCollection.Insert(dateRecord);
            }
            else
            {
                existingDateRecord.Comment = dateRecord.Comment;
                dayCommentCollection.Update(existingDateRecord);
            }
        }

        public void Delete(DateRecord dateRecord)
        {
            if (dateRecord == null) throw new ArgumentNullException(nameof(dateRecord));

            if (dateRecord.Id <= 0)
                throw new PersistenceException("The id of the comment record should be a positive integer.");

            ILiteCollection<DateRecord> dayCommentCollection = database.GetCollection<DateRecord>(CollectionName);

            bool exists = dayCommentCollection
                .Find(x => x.Id == dateRecord.Id)
                .Any();

            if (!exists)
                throw new PersistenceException("There is no record with the specified id to update.");

            dayCommentCollection.Delete(dateRecord.Id);
        }

        public DateRecord GetById(int id)
        {
            ILiteCollection<DateRecord> dayCommentCollection = database.GetCollection<DateRecord>(CollectionName);

            return dayCommentCollection
                .Find(x => x.Id == id)
                .FirstOrDefault();
        }

        public DateRecord GetByDate(DateTime date)
        {
            ILiteCollection<DateRecord> dayCommentCollection = database.GetCollection<DateRecord>(CollectionName);

            return dayCommentCollection
                .Find(x => x.Date == date)
                .FirstOrDefault();
        }

        public List<DateRecord> GetByDate(DateTime startDate, DateTime endDate)
        {
            ILiteCollection<DateRecord> dayCommentCollection = database.GetCollection<DateRecord>(CollectionName);

            return dayCommentCollection
                .Find(x => x.Date >= startDate && x.Date <= endDate)
                .ToList();
        }

        public IList<DateRecord> GetAll()
        {
            ILiteCollection<DateRecord> dayCommentCollection = database.GetCollection<DateRecord>(CollectionName);

            return dayCommentCollection
                .FindAll()
                .ToList();
        }
    }
}
