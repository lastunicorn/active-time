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
using System.Data.Common;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.PersistenceModule.AdoRepositories
{
    public class DayCommentRepository : IDayCommentRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public DayCommentRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            this.unitOfWork = unitOfWork;
        }

        public void Add(DayComment comment)
        {
            AddInternal(comment);

            unitOfWork.Commit();
        }

        private void AddInternal(DayComment comment)
        {
            string sql = string.Format("insert into comments(date,comment) values('{0}', '{1}')",
                comment.Date.ToString("yyyy-MM-dd"),
                SqlTextEncode(comment.Comment));

            using (DbCommand command = unitOfWork.Connection.CreateCommand())
            {
                command.CommandText = sql;

                if (command.ExecuteNonQuery() == 0)
                    throw new Exception();
            }
        }

        public void Update(DayComment comment)
        {
            UpdateInternal(comment);

            unitOfWork.Commit();
        }

        private void UpdateInternal(DayComment comment)
        {
            string sql = string.Format("update comments set comment='{0}' where date='{1}'",
                SqlTextEncode(comment.Comment),
                comment.Date.ToString("yyyy-MM-dd"));

            using (DbCommand command = unitOfWork.Connection.CreateCommand())
            {
                command.CommandText = sql;

                if (command.ExecuteNonQuery() == 0)
                    throw new Exception();
            }
        }

        public void AddOrUpdate(DayComment comment)
        {
            string sql = string.Format("select date from comments where date='{0}'",
                comment.Date.ToString("yyyy-MM-dd"));

            using (DbCommand command = unitOfWork.Connection.CreateCommand())
            {
                command.CommandText = sql;

                DbDataReader reader = command.ExecuteReader();

                if (reader.Read())
                    UpdateInternal(comment);
                else
                    AddInternal(comment);
            }

            unitOfWork.Commit();
        }

        public void Delete(DayComment comment)
        {
            throw new NotImplementedException();
        }

        public DayComment GetById(int id)
        {
            throw new NotImplementedException();
        }

        public DayComment GetByDate(DateTime date)
        {
            string sql = string.Format("select comment from comments where date='{0}'", date.ToString("yyyy-MM-dd"));

            using (DbCommand command = unitOfWork.Connection.CreateCommand())
            {
                command.CommandText = sql;

                DbDataReader reader = command.ExecuteReader();

                bool successfullyRead = reader.Read();

                if (!successfullyRead)
                    return null;

                return new DayComment
                {
                    Date = date,
                    Comment = (string)reader["comment"]
                };
            }
        }

        public IList<DayComment> GetAll()
        {
            throw new NotImplementedException();
        }

        private string SqlTextEncode(string text)
        {
            return text.Replace("'", "''");
        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.Data.SQLite;
//using DustInTheWind.ActiveTime.Common.Persistence;

//namespace DustInTheWind.ActiveTime.PersistenceModule.AdoRepositories
//{
//    public class DayCommentRepository : IDayCommentRepository
//    {
//        private const string ConnectionString = "Data Source=db.s3db";

//        public void Add(DayComment comment)
//        {
//            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
//            {
//                connection.Open();

//                AddInternal(comment, connection);
//            }
//        }

//        private void AddInternal(DayComment comment, SQLiteConnection connection)
//        {
//            string sql = string.Format("insert into comments(date,comment) values('{0}', '{1}')",
//                comment.Date.ToString("yyyy-MM-dd"),
//                SqlTextEncode(comment.Comment));

//            using (SQLiteCommand cmdInsert = new SQLiteCommand(sql, connection))
//            {
//                if (cmdInsert.ExecuteNonQuery() == 0)
//                    throw new Exception();
//            }
//        }

//        public void Update(DayComment comment)
//        {
//            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
//            {
//                connection.Open();

//                UpdateInternal(comment, connection);
//            }
//        }

//        private void UpdateInternal(DayComment comment, SQLiteConnection connection)
//        {
//            string sql = string.Format("update comments set comment='{0}' where date='{1}'",
//                SqlTextEncode(comment.Comment),
//                comment.Date.ToString("yyyy-MM-dd"));

//            using (SQLiteCommand cmdUpdate = new SQLiteCommand(sql, connection))
//            {
//                if (cmdUpdate.ExecuteNonQuery() == 0)
//                    throw new Exception();
//            }
//        }

//        public void AddOrUpdate(DayComment comment)
//        {
//            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
//            {
//                connection.Open();

//                string sql = string.Format("select date from comments where date='{0}'",
//                    comment.Date.ToString("yyyy-MM-dd"));

//                using (SQLiteCommand cmdSelect = new SQLiteCommand(sql, connection))
//                {
//                    SQLiteDataReader reader = cmdSelect.ExecuteReader();

//                    if (reader.Read())
//                        UpdateInternal(comment, connection);
//                    else
//                        AddInternal(comment, connection);
//                }
//            }
//        }

//        public void Delete(DayComment comment)
//        {
//            throw new NotImplementedException();
//        }

//        public DayComment GetById(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public DayComment GetByDate(DateTime date)
//        {
//            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
//            {
//                connection.Open();

//                string sql = string.Format("select comment from comments where date='{0}'", date.ToString("yyyy-MM-dd"));

//                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
//                {
//                    SQLiteDataReader reader = command.ExecuteReader();

//                    bool successfullyRead = reader.Read();

//                    if (!successfullyRead)
//                        return null;

//                    return new DayComment
//                    {
//                        Date = date,
//                        Comment = (string)reader["comment"]
//                    };
//                }
//            }
//        }

//        public IList<DayComment> GetAll()
//        {
//            throw new NotImplementedException();
//        }

//        private string SqlTextEncode(string text)
//        {
//            return text.Replace("'", "''");
//        }
//    }
//}
