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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.Persistence.SQLite.AdoNet.Module.Repositories
{
    public class TimeRecordRepository : ITimeRecordRepository
    {
        private readonly DbConnection connection;

        public TimeRecordRepository(DbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            this.connection = connection;
        }

        public void Add(TimeRecord timeRecord)
        {
            if (timeRecord == null) throw new ArgumentNullException(nameof(timeRecord));

            using (DbCommand command = connection.CreateCommand())
            {
                try
                {
                    string sql = string.Format("insert into records(date,start_time,end_time,type) values('{0}', '{1}', '{2}', {3})",
                        timeRecord.Date.ToString("yyyy-MM-dd"),
                        timeRecord.StartTime.ToString(@"hh\:mm\:ss"),
                        timeRecord.EndTime.ToString(@"hh\:mm\:ss"),
                        (int)timeRecord.RecordType);

                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string errorMessage = string.Format("Error adding the time record '{0}' into the database.", timeRecord);
                    throw new PersistenceException(errorMessage, ex);
                }

                try
                {
                    long lastId = RetrieveLastInsertedId();
                    timeRecord.Id = Convert.ToInt32(lastId);
                }
                catch (Exception ex)
                {
                    throw new PersistenceException("Error retrieving the last inserted id.", ex);
                }
            }
        }

        private long RetrieveLastInsertedId()
        {
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = "select last_insert_rowid()";
                object lastIdAsObject = command.ExecuteScalar();
                return Convert.ToInt64(lastIdAsObject);
            }
        }

        public void Update(TimeRecord timeRecord)
        {
            if (timeRecord == null) throw new ArgumentNullException(nameof(timeRecord));

            if (timeRecord.Id <= 0)
                throw new PersistenceException("The id of the time record should be a positive integer.");

            using (DbCommand command = connection.CreateCommand())
            {
                int recordCount;

                try
                {
                    string sql = string.Format("update records set date='{1}', start_time='{2}', end_time='{3}', type='{4}' where id ={0}",
                        timeRecord.Id,
                        timeRecord.Date.ToString("yyyy-MM-dd"),
                        timeRecord.StartTime.ToString(@"hh\:mm\:ss"),
                        timeRecord.EndTime.ToString(@"hh\:mm\:ss"),
                        (int)timeRecord.RecordType);

                    command.CommandText = sql;
                    recordCount = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string errorMessage = string.Format("Error updating the time record '{0}' into the database.", timeRecord);
                    throw new PersistenceException(errorMessage, ex);
                }

                if (recordCount == 0)
                    throw new PersistenceException("There is no record with the specified id to update.");
            }
        }

        public void Delete(TimeRecord timeRecord)
        {
            if (timeRecord == null) throw new ArgumentNullException(nameof(timeRecord));

            if (timeRecord.Id <= 0)
                throw new PersistenceException("The id of the time record should be a positive integer.");

            using (DbCommand command = connection.CreateCommand())
            {
                int recordCount;

                try
                {
                    string sql = string.Format("delete from records where id='{0}'", timeRecord.Id);

                    command.CommandText = sql;
                    recordCount = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string errorMessage = string.Format("Error deleting the time record '{0}' from the database.", timeRecord);
                    throw new PersistenceException(errorMessage, ex);
                }

                if (recordCount == 0)
                    throw new PersistenceException("There is no record with the specified id to update.");
            }
        }

        public TimeRecord GetById(int id)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = string.Format("select * from records where id={0}", id);

                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    return dataReader.Read()
                        ? ReadCurrentTimeRecord(dataReader)
                        : null;
                }
            }
        }

        public IEnumerable<TimeRecord> GetByDate(DateTime date)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = string.Format("select * from records where date='{0:yyyy-MM-dd}'", date);

                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                        yield return ReadCurrentTimeRecord(dataReader);
                }
            }
        }

        public IEnumerable<TimeRecord> GetAll()
        {
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = "select * from records order by date";

                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                        yield return ReadCurrentTimeRecord(dataReader);
                }
            }
        }

        private static TimeRecord ReadCurrentTimeRecord(DbDataReader dataReader)
        {
            CustomDataReader customDataReader = new CustomDataReader(dataReader);

            int id = customDataReader.ReadInt32FromCurrentTimeRecord("id");
            DateTime date = customDataReader.ReadDateTimeFromCurrentTimeRecord("date");
            TimeSpan startTime = customDataReader.ReadTimeOfDayFromCurrentTimeRecord("start_time");
            TimeSpan endTime = customDataReader.ReadTimeOfDayFromCurrentTimeRecord("end_time");
            TimeRecordType timeRecordType = customDataReader.ReadEnumFromCurrentTimeRecord<TimeRecordType>("type");

            return new TimeRecord
            {
                Id = id,
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                RecordType = timeRecordType
            };
        }
    }
}
