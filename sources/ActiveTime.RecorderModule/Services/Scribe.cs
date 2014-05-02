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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Services;

namespace DustInTheWind.ActiveTime.RecorderModule.Services
{
    /// <summary>
    /// Keeps track of a current record and updates it in the database when requested.
    /// </summary>
    /// <remarks>
    /// The current record can be obtained in two ways: 1) from the database or 2) by creating a new one.
    /// </remarks>
    class Scribe : IScribe
    {
        private readonly ITimeRecordRepository timeRecordRepository;
        private readonly ITimeProvider timeProvider;
        private TimeRecord timeRecord;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scribe"/> class.
        /// </summary>
        /// <param name="timeRecordRepository"></param>
        /// <param name="timeProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Scribe(ITimeRecordRepository timeRecordRepository, ITimeProvider timeProvider)
        {
            if (timeRecordRepository == null)
                throw new ArgumentNullException("timeRecordRepository");

            if (timeProvider == null)
                throw new ArgumentNullException("timeProvider");

            this.timeRecordRepository = timeRecordRepository;
            this.timeProvider = timeProvider;
        }

        /// <summary>
        /// Creates a new record and saves it in the repository.
        /// </summary>
        public void StampNew()
        {
            CreateNewTimeRecordAndSave();
        }

        private void CreateNewTimeRecordAndSave()
        {
            DateTime now = timeProvider.GetDateTime();
            TimeRecord newTimeRecord = new TimeRecord
                                    {
                                        RecordType = TimeRecordType.Normal,
                                        Date = now.Date,
                                        StartTime = now.TimeOfDay,
                                        EndTime = now.TimeOfDay
                                    };
            timeRecordRepository.Add(newTimeRecord);
            timeRecord = newTimeRecord;
        }

        /// <summary>
        /// Updates the current record with the current time and saves it into the
        /// repository. If there is no record a new one is automatically created.
        /// </summary>
        public void Stamp()
        {
            if (timeRecord == null)
                CreateNewTimeRecordAndSave();
            else
                UpdateTimeRecordAndSave();
        }

        private void UpdateTimeRecordAndSave()
        {
            DateTime now = timeProvider.GetDateTime();
            timeRecord.EndTime = now.TimeOfDay;
            timeRecordRepository.Update(timeRecord);
        }

        /// <summary>
        /// Deletes from the database the current time record.
        /// If no time record was created, nothing happens.
        /// </summary>
        public void DeleteCurrentTimeRecord()
        {
            if (timeRecord == null)
                return;

            timeRecordRepository.Delete(timeRecord);
            timeRecord = null;
        }

        /// <summary>
        /// Returns the time passed from the last time the current TimeRecord was stamped.
        /// </summary>
        /// <returns>A <see cref="TimeSpan"/> object representing the time passed from the last time the
        /// current TimeRecord was stamped.</returns>
        public TimeSpan? GetTimeFromLastStamp()
        {
            if (timeRecord == null)
                return null;

            DateTime now = timeProvider.GetDateTime();

            bool isSameDay = timeRecord.Date == now.Date;

            if (isSameDay)
            {
                DateTime endDateTime = now.Date.Add(timeRecord.EndTime);
                return now - endDateTime;
            }

            TimeSpan timeInLastDayAfterEndTime = TimeSpan.FromDays(1) - timeRecord.EndTime;
            TimeSpan b = now.Date - timeRecord.Date + TimeSpan.FromDays(1); // what is b?
            TimeSpan timeInCurrentDay = now.TimeOfDay;

            return timeInLastDayAfterEndTime + b + timeInCurrentDay;
        }
    }
}
