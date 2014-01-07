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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.RecorderModule.Services
{
    /// <summary>
    /// Keeps track of a current record and updates it in the database when requested.
    /// </summary>
    /// <remarks>
    /// The current record can be obtained in two ways: 1) from the database or 2) created new.
    /// </remarks>
    class Scribe : IScribe
    {
        private readonly ITimeRecordRepository repository;
        private readonly ITimeProvider timeProvider;
        private TimeRecord record;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scribe"/> class.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="timeProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Scribe(ITimeRecordRepository repository, ITimeProvider timeProvider)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            if (timeProvider == null)
                throw new ArgumentNullException("timeProvider");

            this.repository = repository;
            this.timeProvider = timeProvider;
        }

        /// <summary>
        /// Creates a new record and saves it in the repository.
        /// </summary>
        public void StampNew()
        {
            CreateNewRecordInternal();
        }

        /// <summary>
        /// Updates the current record with the current time and saves it into the
        /// repository. If there is no record a new one is automatically created.
        /// </summary>
        public void Stamp()
        {
            if (record == null)
                CreateNewRecordInternal();
            else
            {
                DateTime now = timeProvider.GetDateTime();
                record.EndTime = now.TimeOfDay;
                repository.Update(record);
            }
        }

        /// <summary>
        /// Creates a new record with default values and saves it into the repository.
        /// </summary>
        private void CreateNewRecordInternal()
        {
            DateTime now = timeProvider.GetDateTime();
            TimeRecord newRecord = new TimeRecord
                                    {
                                        RecordType = TimeRecordType.Normal,
                                        Date = now.Date,
                                        StartTime = now.TimeOfDay,
                                        EndTime = now.TimeOfDay
                                    };
            repository.Add(newRecord);
            record = newRecord;
        }

        private bool IsNewDay()
        {
            DateTime now = timeProvider.GetDateTime();
            return record != null && record.Date != now.Date;
        }


        public void DeleteDatabaseRecord()
        {
            if (record == null)
                return;

            repository.Delete(record);
            record = null;
        }

        public TimeSpan? GetTimeFromLastStamp()
        {
            if (record == null) return null;

            DateTime now = timeProvider.GetDateTime();

            if (record.Date < now.Date)
            {
                TimeSpan a = TimeSpan.FromDays(1) - record.EndTime;
                TimeSpan b = now.Date - record.Date + TimeSpan.FromDays(1);
                TimeSpan c = now.TimeOfDay;

                return a + b + c;
            }
            else
            {
                return now - now.Date.Add(record.EndTime);
            }
        }
    }
}
