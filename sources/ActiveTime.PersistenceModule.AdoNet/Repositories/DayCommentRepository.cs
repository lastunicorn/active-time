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
using System.Data.SQLite;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.PersistenceModule.AdoNet.Repositories
{
    public class DayCommentRepository : IDayCommentRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public DayCommentRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));

            this.unitOfWork = unitOfWork;
        }

        public void Add(DayComment comment)
        {
            unitOfWork.ExecuteAndCommit(() => AddInternal(comment));
        }

        private void AddInternal(DayComment comment)
        {
            unitOfWork.ExecuteCommand((command) =>
            {
                string sql = string.Format("insert into comments(date,comment) values('{0}', '{1}')",
                    comment.Date.ToString("yyyy-MM-dd"),
                    SqlTextEncode(comment.Comment));

                command.CommandText = sql;

                if (command.ExecuteNonQuery() == 0)
                    throw new Exception();
            });
        }

        public void Update(DayComment comment)
        {
            unitOfWork.ExecuteAndCommit(() => UpdateInternal(comment));
        }

        private void UpdateInternal(DayComment comment)
        {
            unitOfWork.ExecuteCommand((command) =>
            {
                string sql = string.Format("update comments set comment='{0}' where date='{1}'",
                    SqlTextEncode(comment.Comment),
                    comment.Date.ToString("yyyy-MM-dd"));

                command.CommandText = sql;

                if (command.ExecuteNonQuery() == 0)
                    throw new Exception();
            });
        }

        public void AddOrUpdate(DayComment comment)
        {
            unitOfWork.ExecuteAndCommit(() =>
            {
                bool existsRecord = ExistsRecord(comment);

                if (existsRecord)
                    UpdateInternal(comment);
                else
                    AddInternal(comment);
            });
        }

        public bool ExistsRecord(DayComment comment)
        {
            return unitOfWork.ExecuteCommand((command) =>
            {
                string sql = string.Format("select count(*) from comments where date='{0}'",
                    comment.Date.ToString("yyyy-MM-dd"));

                command.CommandText = sql;

                object countAsObject = command.ExecuteScalar();
                int count = int.Parse(countAsObject.ToString());

                return count > 0;
            });
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
            return unitOfWork.ExecuteCommandAndCommit((command) =>
            {
                string sql = string.Format("select comment from comments where date='{0}'", date.ToString("yyyy-MM-dd"));

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
            });
        }

        public List<DayComment> GetByDate(DateTime startDate, DateTime endDate)
        {
            return unitOfWork.ExecuteCommandAndCommit((command) =>
            {
                const string sql = "select date, comment from comments where date >= @startDate and date <= @endDate";

                command.CommandText = sql;

                command.Parameters.Add(new SQLiteParameter
                {
                    ParameterName = "@startDate",
                    Value = startDate
                });

                command.Parameters.Add(new SQLiteParameter
                {
                    ParameterName = "@endDate",
                    Value = endDate
                });

                DbDataReader reader = command.ExecuteReader();

                List<DayComment> dayComments = new List<DayComment>();

                while (reader.Read())
                {
                    DayComment dayComment = new DayComment
                    {
                        Date = (DateTime)reader["date"],
                        Comment = (string)reader["comment"]
                    };

                    dayComments.Add(dayComment);
                }

                return dayComments;
            });
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
