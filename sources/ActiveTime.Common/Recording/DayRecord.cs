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
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.Common.Recording
{
    /// <summary>
    /// Contains activity information for a single day.
    /// </summary>
    public class DayRecord
    {
        /// <summary>
        /// The date for which the current instance contains information.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Gets the date for which the current instance contains information.
        /// </summary>
        public DateTime Date
        {
            get { return date; }
        }

        /// <summary>
        /// The records representing the active time.
        /// </summary>
        private List<DayTimeInterval> activeTimeRecords;

        /// <summary>
        /// Gets or sets the records representing the active time.
        /// </summary>
        public List<DayTimeInterval> ActiveTimeRecords
        {
            get { return activeTimeRecords; }
            //set { activeTimeRecords = value; }
        }

        private string comment;

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public bool IsEmpty
        {
            get { return (activeTimeRecords == null || activeTimeRecords.Count == 0) && (comment == null || comment.Length == 0); }
        }

        public bool HasRecords
        {
            get { return activeTimeRecords != null && activeTimeRecords.Count > 0; }
        }

        public bool HasComment
        {
            get { return comment != null && comment.Length > 0; }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DayRecord"/> class with
        /// the current date.
        /// </summary>
        public DayRecord()
            : this(DateTime.Today)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DayRecord"/> class with
        /// the date for which it contains information.
        /// </summary>
        /// <param name="date">The date for which the new instance contains information.</param>
        public DayRecord(DateTime date)
        {
            this.date = date;
            activeTimeRecords = new List<DayTimeInterval>();
        }

        #endregion

        public TimeSpan GetTotalActiveTime()
        {
            TimeSpan totalTime = TimeSpan.Zero;

            if (activeTimeRecords != null)
            {
                foreach (DayTimeInterval record in activeTimeRecords)
                {
                    totalTime += record.EndTime - record.StartTime;
                }
            }

            return totalTime;
        }

        public TimeSpan GetTotalTime()
        {
            TimeSpan totalTime = TimeSpan.Zero;

            if (activeTimeRecords != null && activeTimeRecords.Count > 0)
            {
                TimeSpan beginHour = activeTimeRecords[0].StartTime;
                TimeSpan endHour = activeTimeRecords[0].EndTime;

                foreach (DayTimeInterval record in activeTimeRecords)
                {
                    if (record.StartTime < beginHour)
                        beginHour = record.StartTime;

                    if (record.EndTime > endHour)
                        endHour = record.EndTime;
                }

                totalTime = endHour - beginHour;
            }

            return totalTime;
        }

        public TimeSpan? GetBeginTime()
        {
            if (activeTimeRecords != null && activeTimeRecords.Count > 0)
            {
                return activeTimeRecords[0].StartTime;
            }
            else
            {
                return null;
            }
        }

        //public DayTimeInterval CalculateLunchBreak(DayRecord dayRecord)
        //{
        //    if (dayRecord == null || dayRecord.Records == null || dayRecord.Records.Length < 2)
        //        return null; // no lunch break.

        //    TimeSpan breakStartHour = dayRecord.Records[0].EndTime;
        //    TimeSpan breakEndHour = dayRecord.Records[0].EndTime;

        //    List<DayTimeInterval> breaks = new List<DayTimeInterval>();

        //    foreach (Record record in dayRecord.Records)
        //    {
        //        breakEndHour = record.StartTime;
        //        breaks.Add(new DayTimeInterval(breakStartHour, breakEndHour));

        //        breakStartHour = record.EndTime;
        //    }
        //}

        public DayTimeInterval[] GetTimeRecords(bool includeBreaks)
        {
            // todo: possible a bug.
            if (activeTimeRecords == null || activeTimeRecords.Count == 0 || !includeBreaks)
                return activeTimeRecords.ToArray();


            List<DayTimeInterval> allRecords = new List<DayTimeInterval>();

            allRecords.Add(activeTimeRecords[0]);

            DayTimeInterval previousRecord = activeTimeRecords[0];

            foreach (DayTimeInterval record in activeTimeRecords)
            {
                allRecords.Add(new Break(previousRecord.EndTime, record.StartTime));
                allRecords.Add(record);
                previousRecord = record;
            }

            return allRecords.ToArray();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DayRecord"/> class and populates it with the
        /// time record received as parameter.
        /// </summary>
        /// <param name="timeRecords"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static DayRecord FromTimeRecords(IList<TimeRecord> timeRecords)
        {
            if (timeRecords == null)
                throw new ArgumentNullException("timeRecords");

            //if (timeRecords.Count == 0)
            //    throw new ArgumentException("The list of TimeRecords contains no items.", "timeRecords");

            DayRecord dayRecord = null;

            foreach (TimeRecord timeRecord in timeRecords)
            {
                if (timeRecord == null)
                    throw new ArgumentException("The list of TimeRecords contains null items.", "timeRecords");

                if (dayRecord == null)
                    dayRecord = new DayRecord(timeRecord.Date);

                dayRecord.ActiveTimeRecords.Add(new DayTimeInterval(timeRecord.StartTime, timeRecord.EndTime));
            }

            return dayRecord;
        }
    }
}
