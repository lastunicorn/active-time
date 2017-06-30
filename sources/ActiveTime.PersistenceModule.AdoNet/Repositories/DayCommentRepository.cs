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

namespace DustInTheWind.ActiveTime.PersistenceModule.SQLite.AdoNet.Repositories
{
    public class DayCommentRepository : IDayCommentRepository
    {
        private readonly DbConnection connection;

        public DayCommentRepository(DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            this.connection = connection;
        }

        public void Add(DayComment comment)
        {
            AddInternal(comment);
        }

        private void AddInternal(DayComment comment)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                string sql = string.Format("insert into comments(date,comment) values('{0:yyyy-MM-dd}', '{1}')",
                    comment.Date,
                    SqlTextEncode(comment.Comment));

                command.CommandText = sql;

                if (command.ExecuteNonQuery() == 0)
                    throw new Exception();
            }
        }

        public void Update(DayComment comment)
        {
            UpdateInternal(comment);
        }

        private void UpdateInternal(DayComment comment)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                string sql = string.Format("update comments set comment='{0}' where date='{1:yyyy-MM-dd}'",
                    SqlTextEncode(comment.Comment),
                    comment.Date);

                command.CommandText = sql;

                if (command.ExecuteNonQuery() == 0)
                    throw new Exception();
            }
        }

        public void AddOrUpdate(DayComment comment)
        {
            bool existsRecord = ExistsRecord(comment);

            if (existsRecord)
                UpdateInternal(comment);
            else
                AddInternal(comment);
        }

        public bool ExistsRecord(DayComment comment)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                string sql = string.Format("select count(*) from comments where date='{0:yyyy-MM-dd}'", comment.Date);

                command.CommandText = sql;

                object countAsObject = command.ExecuteScalar();
                int count = int.Parse(countAsObject.ToString());

                return count > 0;
            }
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
            using (DbCommand command = connection.CreateCommand())
            {
                string sql = string.Format("select comment from comments where date='{0:yyyy-MM-dd}'", date);

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

        public List<DayComment> GetByDate(DateTime startDate, DateTime endDate)
        {
            using (DbCommand command = connection.CreateCommand())
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

                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    List<DayComment> dayComments = new List<DayComment>();

                    while (dataReader.Read())
                    {
                        DayComment dayComment = new DayComment
                        {
                            Date = (DateTime)dataReader["date"],
                            Comment = (string)dataReader["comment"]
                        };

                        dayComments.Add(dayComment);
                    }

                    return dayComments;
                }
            }
        }

        public IList<DayComment> GetAll()
        {
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = "select * from comments";

                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    List<DayComment> dayComments = new List<DayComment>();

                    while (dataReader.Read())
                    {
                        DayComment dayComment = new DayComment
                        {
                            Date = (DateTime)dataReader["date"],
                            Comment = (string)dataReader["comment"]
                        };

                        dayComments.Add(dayComment);
                    }

                    return dayComments;
                }
            }
        }

        private string SqlTextEncode(string text)
        {
            return text.Replace("'", "''");
        }
    }
}