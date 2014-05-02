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
    public class TimeRecordRepository : ITimeRecordRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public TimeRecordRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            this.unitOfWork = unitOfWork;
        }

        public void Add(TimeRecord timeRecord)
        {
            string sql = string.Format("insert into records(date,start_time,end_time,type) values('{0}', '{1}', '{2}', {3})",
                timeRecord.Date.ToString("yyyy-MM-dd"),
                timeRecord.StartTime.ToString(@"hh\:mm\:ss"),
                timeRecord.EndTime.ToString(@"hh\:mm\:ss"),
                (int)timeRecord.RecordType);

            try
            {
                ExecuteNonQuery(sql);
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

            unitOfWork.Commit();
        }

        private int ExecuteNonQuery(string sql)
        {
            using (DbCommand command = unitOfWork.Connection.CreateCommand())
            {
                command.CommandText = sql;
                return command.ExecuteNonQuery();
            }
        }

        private long RetrieveLastInsertedId()
        {
            using (DbCommand command = unitOfWork.Connection.CreateCommand())
            {
                command.CommandText = "select last_insert_rowid()";
                object lastIdAsObject = command.ExecuteScalar();
                return Convert.ToInt64(lastIdAsObject);
            }
        }

        public void Update(TimeRecord timeRecord)
        {
            if (timeRecord == null)
                throw new ArgumentNullException("timeRecord");

            if (timeRecord.Id <= 0)
                throw new PersistenceException("The id of the time record should be a positive integer.");

            //string sql = string.Format("update records set end_time='{0}' where date='{1}' and start_time='{2}'",
            //    record.EndTime.ToString(@"hh\:mm\:ss"),
            //    record.Date.ToString("yyyy-MM-dd"),
            //    record.StartTime.ToString(@"hh\:mm\:ss"));

            string sql = string.Format("update records set date='{1}', start_time='{2}', end_time='{3}', type='{4}' where id ={0}",
                timeRecord.Id,
                timeRecord.Date.ToString("yyyy-MM-dd"),
                timeRecord.StartTime.ToString(@"hh\:mm\:ss"),
                timeRecord.EndTime.ToString(@"hh\:mm\:ss"),
                (int)timeRecord.RecordType);

            int recordCount = ExecuteNonQuery(sql);

            if (recordCount == 0)
                throw new PersistenceException("There is no record with the specified id to update.");

            unitOfWork.Commit();
        }

        public void Delete(TimeRecord timeRecord)
        {
            string sql = string.Format("delete from records where date='{0}' and start_time='{1}' and end_time='{2}'",
                timeRecord.Date.ToString("yyyy-MM-dd"),
                timeRecord.StartTime.ToString(@"hh\:mm\:ss"),
                timeRecord.EndTime.ToString(@"hh\:mm\:ss"));

            ExecuteNonQuery(sql);

            unitOfWork.Commit();
        }

        public TimeRecord GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<TimeRecord> GetByDate(DateTime date)
        {
            string sql = string.Format("select id, start_time, end_time from records where date='{0}'", date.ToString("yyyy-MM-dd"));

            using (DbCommand command = unitOfWork.Connection.CreateCommand())
            {
                command.CommandText = sql;
                using (DbDataReader reader = command.ExecuteReader())
                {
                    List<TimeRecord> records = new List<TimeRecord>();

                    while (reader.Read())
                    {
                        object idAsObject = reader["id"];
                        object startTimeAsObject = reader["start_time"];
                        object endTimeAsObject = reader["end_time"];

                        TimeRecord timeRecord = new TimeRecord
                        {
                            Id = int.Parse(idAsObject.ToString()),
                            Date = date,
                            StartTime = DateTime.Parse(startTimeAsObject.ToString()).TimeOfDay,
                            EndTime = DateTime.Parse(endTimeAsObject.ToString()).TimeOfDay,
                        };

                        records.Add(timeRecord);
                    }

                    return records.ToArray();
                }
            }
        }
    }
}
