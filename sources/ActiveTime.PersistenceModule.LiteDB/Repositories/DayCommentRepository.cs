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
    internal class DayCommentRepository : IDayCommentRepository
    {
        public const string CollectionName = "DayComment";

        private readonly LiteDatabase database;

        public DayCommentRepository(LiteDatabase database)
        {
            if (database == null) throw new ArgumentNullException(nameof(database));
            this.database = database;
        }

        public void Add(DayComment comment)
        {
            LiteCollection<DayComment> dayCommentCollection = database.GetCollection<DayComment>(CollectionName);
            dayCommentCollection.Insert(comment);
        }

        public void Update(DayComment comment)
        {
            LiteCollection<DayComment> dayCommentCollection = database.GetCollection<DayComment>(CollectionName);
            dayCommentCollection.Update(comment);
        }

        public void AddOrUpdate(DayComment comment)
        {
            LiteCollection<DayComment> dayCommentCollection = database.GetCollection<DayComment>(CollectionName);

            DayComment existingComment = dayCommentCollection
                .Find(x => x.Date == comment.Date)
                .FirstOrDefault();

            if (existingComment == null)
            {
                dayCommentCollection.Insert(comment);
            }
            else
            {
                existingComment.Comment = comment.Comment;
                dayCommentCollection.Update(existingComment);
            }
        }

        public void Delete(DayComment comment)
        {
            LiteCollection<DayComment> dayCommentCollection = database.GetCollection<DayComment>(CollectionName);
            dayCommentCollection.Delete(x => x.Id == comment.Id);
        }

        public DayComment GetById(int id)
        {
            LiteCollection<DayComment> dayCommentCollection = database.GetCollection<DayComment>(CollectionName);

            return dayCommentCollection
                .Find(x => x.Id == id)
                .FirstOrDefault();
        }

        public DayComment GetByDate(DateTime date)
        {
            LiteCollection<DayComment> dayCommentCollection = database.GetCollection<DayComment>(CollectionName);

            return dayCommentCollection
                .Find(x => x.Date == date)
                .FirstOrDefault();
        }

        public List<DayComment> GetByDate(DateTime startDate, DateTime endDate)
        {
            LiteCollection<DayComment> dayCommentCollection = database.GetCollection<DayComment>(CollectionName);

            return dayCommentCollection
                .Find(x => x.Date >= startDate && x.Date <= endDate)
                .ToList();
        }

        public IList<DayComment> GetAll()
        {
            LiteCollection<DayComment> dayCommentCollection = database.GetCollection<DayComment>(CollectionName);

            return dayCommentCollection
                .FindAll()
                .ToList();
        }
    }
}
