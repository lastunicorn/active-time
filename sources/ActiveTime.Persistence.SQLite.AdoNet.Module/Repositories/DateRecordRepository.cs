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
    public class DateRecordRepository : IDateRecordRepository
    {
        private readonly DbConnection connection;

        public DateRecordRepository(DbConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public void Add(DateRecord dateRecord)
        {
            AddInternal(dateRecord);
        }

        private void AddInternal(DateRecord dateRecord)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                string sql = string.Format("insert into comments(date,comment) values('{0:yyyy-MM-dd}', '{1}')",
                    dateRecord.Date,
                    SqlTextEncode(dateRecord.Comment));

                command.CommandText = sql;

                if (command.ExecuteNonQuery() == 0)
                    throw new Exception();
            }
        }

        public void Update(DateRecord dateRecord)
        {
            UpdateInternal(dateRecord);
        }

        private void UpdateInternal(DateRecord dateRecord)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                string sql = string.Format("update comments set comment='{0}' where date='{1:yyyy-MM-dd}'",
                    SqlTextEncode(dateRecord.Comment),
                    dateRecord.Date);

                command.CommandText = sql;

                if (command.ExecuteNonQuery() == 0)
                    throw new Exception();
            }
        }

        public void AddOrUpdate(DateRecord dateRecord)
        {
            bool existsRecord = ExistsRecord(dateRecord);

            if (existsRecord)
                UpdateInternal(dateRecord);
            else
                AddInternal(dateRecord);
        }

        public bool ExistsRecord(DateRecord dateRecord)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                string sql = string.Format("select count(*) from comments where date='{0:yyyy-MM-dd}'", dateRecord.Date);

                command.CommandText = sql;

                object countAsObject = command.ExecuteScalar();
                int count = int.Parse(countAsObject.ToString());

                return count > 0;
            }
        }

        public void Delete(DateRecord dateRecord)
        {
            throw new NotImplementedException();
        }

        public DateRecord GetById(int id)
        {
            throw new NotImplementedException();
        }

        public DateRecord GetByDate(DateTime date)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                string sql = string.Format("select comment from comments where date='{0:yyyy-MM-dd}'", date);

                command.CommandText = sql;

                DbDataReader reader = command.ExecuteReader();

                bool successfullyRead = reader.Read();

                if (!successfullyRead)
                    return null;

                return new DateRecord
                {
                    Date = date,
                    Comment = (string)reader["comment"]
                };
            }
        }

        public List<DateRecord> GetByDate(DateTime startDate, DateTime endDate)
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
                    List<DateRecord> dayComments = new List<DateRecord>();

                    while (dataReader.Read())
                    {
                        DateRecord dateRecord = new DateRecord
                        {
                            Date = (DateTime)dataReader["date"],
                            Comment = (string)dataReader["comment"]
                        };

                        dayComments.Add(dateRecord);
                    }

                    return dayComments;
                }
            }
        }

        public IList<DateRecord> GetAll()
        {
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = "select * from comments";

                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    List<DateRecord> dayComments = new List<DateRecord>();

                    while (dataReader.Read())
                    {
                        DateRecord dateRecord = new DateRecord
                        {
                            Date = (DateTime)dataReader["date"],
                            Comment = (string)dataReader["comment"]
                        };

                        dayComments.Add(dateRecord);
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