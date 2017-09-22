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
using DustInTheWind.ActiveTime.Persistence;

namespace DustInTheWind.ActiveTime.Recording
{
    /// <summary>
    /// Contains activity information for a single day.
    /// </summary>
    public class DayRecord
    {
        private readonly List<DayTimeInterval> activeTimeRecords;

        public DayRecord(IEnumerable<TimeRecord> timeRecords)
        {
            if (timeRecords == null) throw new ArgumentNullException(nameof(timeRecords));

            activeTimeRecords = new List<DayTimeInterval>();

            foreach (TimeRecord timeRecord in timeRecords)
            {
                if (timeRecord == null)
                    throw new ArgumentException("The list of TimeRecords contains null items.", nameof(timeRecords));

                DayTimeInterval dayTimeInterval = new DayTimeInterval(timeRecord.StartTime, timeRecord.EndTime);
                activeTimeRecords.Add(dayTimeInterval);
            }
        }

        public TimeSpan GetTotalActiveTime()
        {
            return activeTimeRecords.Aggregate(
                TimeSpan.Zero,
                (total, record) => total + (record.EndTime - record.StartTime));
        }

        public TimeSpan GetTotalTime()
        {
            if (activeTimeRecords.Count == 0)
                return TimeSpan.Zero;

            TimeSpan beginHour = activeTimeRecords[0].StartTime;
            TimeSpan endHour = activeTimeRecords[0].EndTime;

            foreach (DayTimeInterval record in activeTimeRecords)
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
            if (activeTimeRecords.Count == 0)
                return null;

            return activeTimeRecords[0].StartTime;
        }

        public TimeSpan? GetEndTime()
        {
            if (activeTimeRecords.Count == 0)
                return null;

            return activeTimeRecords.Last().StartTime;
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
            if (!includeBreaks)
                return activeTimeRecords.ToArray();

            if (activeTimeRecords.Count == 0)
                return new DayTimeInterval[0];

            List<DayTimeInterval> allRecords = new List<DayTimeInterval>();
            DayTimeInterval previousRecord = null;

            foreach (DayTimeInterval record in activeTimeRecords)
            {
                if (previousRecord != null)
                {
                    Break breakRecord = new Break(previousRecord.EndTime, record.StartTime);
                    allRecords.Add(breakRecord);
                }

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

            TimeSpan? previousEnd = null;
            foreach (DayTimeInterval dayTimeInterval in activeTimeRecords)
            {
                if (previousEnd != null)
                {
                    TimeSpan pauseTime = dayTimeInterval.StartTime - previousEnd.Value;
                    consumedPauseTime += pauseTime;

                    existsLunchPause = pauseTime > TimeSpan.FromMinutes(30) && dayTimeInterval.StartTime > TimeSpan.FromHours(11.5);
                }

                previousEnd = dayTimeInterval.EndTime;
            }

            estimatedEndTime += TimeSpan.FromHours(8);

            if (!existsLunchPause)
                estimatedEndTime += TimeSpan.FromHours(1);

            estimatedEndTime += consumedPauseTime;

            return estimatedEndTime;
        }
    }
}
