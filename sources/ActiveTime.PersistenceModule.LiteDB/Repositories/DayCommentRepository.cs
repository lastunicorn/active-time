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
        private const string ConnectionString = Constants.DatabaseFileName;
        private const string CollectionName = "DayComment";

        public void Add(DayComment comment)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                LiteCollection<DayComment> timeRecords = db.GetCollection<DayComment>(CollectionName);
                timeRecords.Insert(comment);
            }
        }

        public void Update(DayComment comment)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                LiteCollection<DayComment> timeRecords = db.GetCollection<DayComment>(CollectionName);
                timeRecords.Update(comment);
            }
        }

        public void AddOrUpdate(DayComment comment)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                LiteCollection<DayComment> timeRecords = db.GetCollection<DayComment>(CollectionName);
                DayComment existingComment = timeRecords.Find(x => x.Date == comment.Date)
                    .FirstOrDefault();

                if (existingComment == null)
                    timeRecords.Insert(comment);
                else
                {
                    existingComment.Comment = comment.Comment;
                    timeRecords.Update(existingComment);
                }
            }
        }

        public void Delete(DayComment comment)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                LiteCollection<DayComment> timeRecords = db.GetCollection<DayComment>(CollectionName);
                timeRecords.Delete(x => x.Id == comment.Id);
            }
        }

        public DayComment GetById(int id)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                LiteCollection<DayComment> timeRecords = db.GetCollection<DayComment>(CollectionName);
                return timeRecords.Find(x => x.Id == id)
                    .FirstOrDefault();
            }
        }

        public DayComment GetByDate(DateTime date)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                LiteCollection<DayComment> timeRecords = db.GetCollection<DayComment>(CollectionName);
                return timeRecords.Find(x => x.Date == date)
                    .FirstOrDefault();
            }
        }

        public List<DayComment> GetByDate(DateTime startDate, DateTime endDate)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                LiteCollection<DayComment> timeRecords = db.GetCollection<DayComment>(CollectionName);
                return timeRecords.Find(x => x.Date >= startDate && x.Date <= endDate)
                    .ToList();
            }
        }

        public IList<DayComment> GetAll()
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                LiteCollection<DayComment> timeRecords = db.GetCollection<DayComment>(CollectionName);
                return timeRecords.FindAll()
                    .ToList();
            }
        }
    }
}
