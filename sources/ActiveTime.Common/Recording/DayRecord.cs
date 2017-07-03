// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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
using System.Linq;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.Common.Recording
{
    /// <summary>
    /// Contains activity information for a single day.
    /// </summary>
    public class DayRecord
    {
        /// <summary>
        /// Gets the date for which the current instance contains information.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Gets or sets the records representing the active time.
        /// </summary>
        public List<DayTimeInterval> ActiveTimeRecords { get; }

        public string Comment { get; set; }

        public bool IsEmpty
        {
            get { return (ActiveTimeRecords == null || ActiveTimeRecords.Count == 0) && string.IsNullOrEmpty(Comment); }
        }

        public bool HasRecords
        {
            get { return ActiveTimeRecords != null && ActiveTimeRecords.Count > 0; }
        }

        public bool HasComment
        {
            get { return !string.IsNullOrEmpty(Comment); }
        }

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
            Date = date;
            ActiveTimeRecords = new List<DayTimeInterval>();
        }

        public TimeSpan GetTotalActiveTime()
        {
            if (ActiveTimeRecords == null)
                return TimeSpan.Zero;

            TimeSpan totalTime = TimeSpan.Zero;

            foreach (DayTimeInterval record in ActiveTimeRecords)
            {
                totalTime += record.EndTime - record.StartTime;
            }

            return totalTime;
        }

        public TimeSpan GetTotalTime()
        {
            bool existsRecords = ActiveTimeRecords != null && ActiveTimeRecords.Count > 0;

            if (!existsRecords)
                return TimeSpan.Zero;

            TimeSpan beginHour = ActiveTimeRecords[0].StartTime;
            TimeSpan endHour = ActiveTimeRecords[0].EndTime;

            foreach (DayTimeInterval record in ActiveTimeRecords)
            {
                if (record.StartTime < beginHour)
                    beginHour = record.StartTime;

                if (record.EndTime > endHour)
                    endHour = record.EndTime;
            }

            return endHour - beginHour;
        }

        public TimeSpan? GetBeginTime()
        {
            bool existsRecords = ActiveTimeRecords != null && ActiveTimeRecords.Count > 0;

            if (!existsRecords)
                return null;

            return ActiveTimeRecords[0].StartTime;
        }

        public TimeSpan? GetEndTime()
        {
            bool existsRecords = ActiveTimeRecords != null && ActiveTimeRecords.Count > 0;

            if (!existsRecords)
                return null;

            return ActiveTimeRecords.Last().StartTime;
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
            if (ActiveTimeRecords == null || ActiveTimeRecords.Count == 0 || !includeBreaks)
                return ActiveTimeRecords.ToArray();


            List<DayTimeInterval> allRecords = new List<DayTimeInterval>();

            allRecords.Add(ActiveTimeRecords[0]);

            DayTimeInterval previousRecord = ActiveTimeRecords[0];

            foreach (DayTimeInterval record in ActiveTimeRecords)
            {
                allRecords.Add(new Break(previousRecord.EndTime, record.StartTime));
                allRecords.Add(record);
                previousRecord = record;
            }

            return allRecords.ToArray();
        }

        public TimeSpan? GetEstimatedEndTime()
        {
            TimeSpan? beginTime = GetBeginTime();

            if (beginTime == null)
                return null;

            TimeSpan? estimatedEndTime = beginTime;

            TimeSpan consumedPauseTime = TimeSpan.Zero;
            bool existsLunchPause = false;

            if (ActiveTimeRecords != null)
            {
                TimeSpan? previousEnd = null;
                foreach (DayTimeInterval dayTimeInterval in ActiveTimeRecords)
                {
                    if (previousEnd != null)
                    {
                        TimeSpan pauseTime = dayTimeInterval.StartTime - previousEnd.Value;
                        consumedPauseTime += pauseTime;

                        existsLunchPause = pauseTime > TimeSpan.FromMinutes(30) && dayTimeInterval.StartTime > TimeSpan.FromHours(11.5);
                    }

                    previousEnd = dayTimeInterval.EndTime;
                }
            }

            estimatedEndTime += TimeSpan.FromHours(8);

            if(!existsLunchPause)
                estimatedEndTime += TimeSpan.FromHours(1);

            estimatedEndTime += consumedPauseTime;

            return estimatedEndTime;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DayRecord"/> class and populates it with the
        /// time record received as parameter.
        /// </summary>
        /// <param name="timeRecords"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static DayRecord FromTimeRecords(IEnumerable<TimeRecord> timeRecords)
        {
            if (timeRecords == null)
                return null;

            //if (timeRecords.Count == 0)
            //    throw new ArgumentException("The list of TimeRecords contains no items.", "timeRecords");

            DayRecord dayRecord = null;

            foreach (TimeRecord timeRecord in timeRecords)
            {
                if (timeRecord == null)
                    throw new ArgumentException("The list of TimeRecords contains null items.", nameof(timeRecords));

                if (dayRecord == null)
                    dayRecord = new DayRecord(timeRecord.Date);

                dayRecord.ActiveTimeRecords.Add(new DayTimeInterval(timeRecord.StartTime, timeRecord.EndTime));
            }

            return dayRecord;
        }
    }
}
