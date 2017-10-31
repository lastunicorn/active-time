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
using DustInTheWind.ActiveTime.Persistence;

namespace DustInTheWind.ActiveTime.Recording
{
    /// <summary>
    /// Analyses the list of records and:
    /// - calculates the estimated end time.
    /// - builds the full time records.
    /// - exposes the start time of the whole list.
    /// - exposes the end time of the whole list.
    /// - calculates the total time.
    /// - calculates the total active time.
    /// </summary>
    internal class RecordAnalyzer
    {
        private readonly List<DayTimeInterval> activeTimeRecords;

        private readonly TimeSpan minLunchTimeStart = TimeSpan.FromHours(11);
        private readonly TimeSpan maxLunchTimeEnd = TimeSpan.FromHours(16);
        private readonly TimeSpan minLunchTimeInterval = TimeSpan.FromMinutes(30);
        private TimeSpan totalBrakeTime;
        private TimeSpan? lunchBreakTime;
        private DayTimeInterval previousRecord;
        private DayTimeInterval currentRecord;

        public TimeSpan? BeginTime { get; private set; }

        public TimeSpan? EndTime { get; private set; }

        public TimeSpan? EstimatedEndTime { get; private set; }

        public RecordAnalyzer(IEnumerable<TimeRecord> timeRecords)
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

        public void Analyze()
        {
            Clear();

            if (activeTimeRecords.Count == 0)
                return;

            BeginTime = activeTimeRecords[0].StartTime;
            EndTime = activeTimeRecords[0].EndTime;

            IEnumerator<DayTimeInterval> enumerator = activeTimeRecords.GetEnumerator();

            while (enumerator.MoveNext())
            {
                previousRecord = currentRecord;
                currentRecord = enumerator.Current;

                UpdateBeginTime();
                UpdateEndTime();

                bool isFirstRecord = previousRecord == null;

                if (!isFirstRecord)
                    UpdateBreakList();
            }

            //foreach (DayTimeInterval record in activeTimeRecords)
            //{
            //    currentRecord = record;

            //    UpdateBeginTime();
            //    UpdateEndTime();

            //    bool isFirstRecord = previousRecord == null;
            //    if (!isFirstRecord)
            //    {
            //        UpdateLunchBreak();
            //    }

            //    previousRecord = currentRecord;
            //}

            EstimatedEndTime = CalculateEstimatedEndTime();
        }

        private void UpdateBreakList()
        {
            TimeSpan breakStartTime = previousRecord.EndTime;
            TimeSpan breakEndTime = currentRecord.StartTime;
            TimeSpan breakTime = breakEndTime - breakStartTime;

            bool isLunchBreak = breakStartTime >= minLunchTimeStart &&
                                breakEndTime <= maxLunchTimeEnd &&
                                breakTime >= minLunchTimeInterval &&
                                (lunchBreakTime == null || breakTime > lunchBreakTime);

            if (isLunchBreak)
            {
                if (lunchBreakTime != null)
                    totalBrakeTime += lunchBreakTime.Value;

                lunchBreakTime = breakTime;
            }
            else
            {
                totalBrakeTime += breakTime;
            }
        }

        private TimeSpan? CalculateEstimatedEndTime()
        {
            return BeginTime +
                TimeSpan.FromHours(7) +
                (totalBrakeTime < TimeSpan.FromHours(1) ? TimeSpan.FromHours(1) : totalBrakeTime) +
                (lunchBreakTime ?? ((EndTime != null && EndTime.Value <= maxLunchTimeEnd - minLunchTimeInterval) ? TimeSpan.FromHours(1) : TimeSpan.Zero));
        }

        private void UpdateEndTime()
        {
            if (currentRecord.EndTime > EndTime)
                EndTime = currentRecord.EndTime;
        }

        private void UpdateBeginTime()
        {
            if (currentRecord.StartTime < BeginTime)
                BeginTime = currentRecord.StartTime;
        }

        private void Clear()
        {
            EstimatedEndTime = null;
            BeginTime = null;
            EndTime = null;

            totalBrakeTime = TimeSpan.Zero;
            lunchBreakTime = null;

            previousRecord = null;
            currentRecord = null;
        }
    }
}