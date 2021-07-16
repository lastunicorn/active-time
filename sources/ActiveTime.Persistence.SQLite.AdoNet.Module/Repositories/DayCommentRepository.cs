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
using System.Data.Common;
using System.Data.SQLite;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.Module.Repositories
{
    public class DayCommentRepository : IDayCommentRepository
    {
        private readonly DbConnection connection;

        public DayCommentRepository(DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            this.connection = connection;
        }

        public void Add(DayRecord dayRecord)
        {
            AddInternal(dayRecord);
        }

        private void AddInternal(DayRecord dayRecord)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                string sql = string.Format("insert into comments(date,comment) values('{0:yyyy-MM-dd}', '{1}')",
                    dayRecord.Date,
                    SqlTextEncode(dayRecord.Comment));

                command.CommandText = sql;

                if (command.ExecuteNonQuery() == 0)
                    throw new Exception();
            }
        }

        public void Update(DayRecord dayRecord)
        {
            UpdateInternal(dayRecord);
        }

        private void UpdateInternal(DayRecord dayRecord)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                string sql = string.Format("update comments set comment='{0}' where date='{1:yyyy-MM-dd}'",
                    SqlTextEncode(dayRecord.Comment),
                    dayRecord.Date);

                command.CommandText = sql;

                if (command.ExecuteNonQuery() == 0)
                    throw new Exception();
            }
        }

        public void AddOrUpdate(DayRecord dayRecord)
        {
            bool existsRecord = ExistsRecord(dayRecord);

            if (existsRecord)
                UpdateInternal(dayRecord);
            else
                AddInternal(dayRecord);
        }

        public bool ExistsRecord(DayRecord dayRecord)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                string sql = string.Format("select count(*) from comments where date='{0:yyyy-MM-dd}'", dayRecord.Date);

                command.CommandText = sql;

                object countAsObject = command.ExecuteScalar();
                int count = int.Parse(countAsObject.ToString());

                return count > 0;
            }
        }

        public void Delete(DayRecord dayRecord)
        {
            throw new NotImplementedException();
        }

        public DayRecord GetById(int id)
        {
            throw new NotImplementedException();
        }

        public DayRecord GetByDate(DateTime date)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                string sql = string.Format("select comment from comments where date='{0:yyyy-MM-dd}'", date);

                command.CommandText = sql;

                DbDataReader reader = command.ExecuteReader();

                bool successfullyRead = reader.Read();

                if (!successfullyRead)
                    return null;

                return new DayRecord
                {
                    Date = date,
                    Comment = (string)reader["comment"]
                };
            }
        }

        public List<DayRecord> GetByDate(DateTime startDate, DateTime endDate)
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
                    List<DayRecord> dayComments = new List<DayRecord>();

                    while (dataReader.Read())
                    {
                        DayRecord dayRecord = new DayRecord
                        {
                            Date = (DateTime)dataReader["date"],
                            Comment = (string)dataReader["comment"]
                        };

                        dayComments.Add(dayRecord);
                    }

                    return dayComments;
                }
            }
        }

        public IList<DayRecord> GetAll()
        {
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = "select * from comments";

                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    List<DayRecord> dayComments = new List<DayRecord>();

                    while (dataReader.Read())
                    {
                        DayRecord dayRecord = new DayRecord
                        {
                            Date = (DateTime)dataReader["date"],
                            Comment = (string)dataReader["comment"]
                        };

                        dayComments.Add(dayRecord);
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