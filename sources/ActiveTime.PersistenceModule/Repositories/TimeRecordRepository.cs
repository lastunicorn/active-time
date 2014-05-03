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
            if (timeRecord == null)
                throw new ArgumentNullException("timeRecord");

            if (timeRecord.Id <= 0)
                throw new PersistenceException("The id of the time record should be a positive integer.");

            string sql = string.Format("delete from records where id='{0}'", timeRecord.Id);

            int recordCount = ExecuteNonQuery(sql);

            if (recordCount == 0)
                throw new PersistenceException("There is no record with the specified id to update.");

            unitOfWork.Commit();
        }

        public TimeRecord GetById(int id)
        {
            using (DbCommand command = unitOfWork.Connection.CreateCommand())
            {
                command.CommandText = string.Format("select * from records where id={0}", id);

                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    if (!dataReader.Read())
                        return null;

                    return ReadCurrentTimeRecord(dataReader);
                }
            }
        }

        public IList<TimeRecord> GetByDate(DateTime date)
        {
            using (DbCommand command = unitOfWork.Connection.CreateCommand())
            {
                command.CommandText = string.Format("select * from records where date='{0}'", date.ToString("yyyy-MM-dd"));

                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    List<TimeRecord> timeRecords = new List<TimeRecord>();

                    while (dataReader.Read())
                    {
                        TimeRecord timeRecord = ReadCurrentTimeRecord(dataReader);
                        timeRecords.Add(timeRecord);
                    }

                    return timeRecords;
                }
            }
        }

        private static TimeRecord ReadCurrentTimeRecord(DbDataReader dataReader)
        {
            object idAsObject = dataReader["id"];
            object dateAsObject = dataReader["date"];
            object startTimeAsObject = dataReader["start_time"];
            object endTimeAsObject = dataReader["end_time"];
            object recordTypeAsObject = dataReader["type"];

            return new TimeRecord
            {
                Id = int.Parse(idAsObject.ToString()),
                Date = DateTime.Parse(dateAsObject.ToString()),
                StartTime = DateTime.Parse(startTimeAsObject.ToString()).TimeOfDay,
                EndTime = DateTime.Parse(endTimeAsObject.ToString()).TimeOfDay,
                RecordType = (TimeRecordType)Enum.Parse(typeof(TimeRecordType), recordTypeAsObject.ToString())
            };
        }
    }
}
