// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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

using DustInTheWind.ActiveTime.Domain;

namespace DustInTheWind.ActiveTime.Application.Recording2;

/// <summary>
/// Contains activity information for a single day.
/// </summary>
/// <remarks>
/// Analyzes the list of records and:
/// - calculates the estimated end time.
/// - builds the full time records.
/// - exposes the start time of the whole list.
/// - exposes the end time of the whole list.
/// - calculates the total time.
/// - calculates the total active time.
/// </remarks>
public class DayRecord
{
    private readonly List<DayTimeInterval> activeIntervals;

    private readonly TimeSpan minLunchTimeStart = TimeSpan.FromHours(11);
    private readonly TimeSpan maxLunchTimeEnd = TimeSpan.FromHours(16);
    private readonly TimeSpan minLunchTimeInterval = TimeSpan.FromMinutes(30);

    private DayTimeInterval previousActiveInterval;
    private DayTimeInterval currentActiveInterval;
    private Break previousBreakInterval;

    public List<DayTimeInterval> AllIntervals { get; }

    public TimeSpan? OverallBeginTime { get; private set; }

    public TimeSpan? OverallEndTime { get; private set; }

    public TimeSpan? EstimatedEndTime { get; private set; }

    public TimeSpan TotalActiveTime { get; private set; }

    public TimeSpan TotalTime { get; private set; }

    public TimeSpan TotalBreakTime { get; private set; }

    public TimeSpan? LunchBreakTime { get; private set; }

    public DayRecord(IEnumerable<TimeRecord> timeRecords)
    {
        if (timeRecords == null) throw new ArgumentNullException(nameof(timeRecords));

        activeIntervals = new List<DayTimeInterval>();
        AllIntervals = new List<DayTimeInterval>();

        foreach (TimeRecord timeRecord in timeRecords)
        {
            if (timeRecord == null)
                throw new ArgumentException("The list of TimeRecords contains null items.", nameof(timeRecords));

            DayTimeInterval dayTimeInterval = new(timeRecord.StartTime, timeRecord.EndTime);
            activeIntervals.Add(dayTimeInterval);
        }

        Analyze();
    }

    private void Analyze()
    {
        Clear();

        using (IEnumerator<DayTimeInterval> enumerator = activeIntervals.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                previousActiveInterval = currentActiveInterval;
                currentActiveInterval = enumerator.Current;
                previousBreakInterval = CalculateBreakInterval();

                UpdateBeginTime();
                UpdateEndTime();
                UpdateActiveTime();
                UpdateBreakTime();

                UpdateAllIntervalList();
            }
        }

        TotalTime = CalculateTotalTime();
        EstimatedEndTime = CalculateEstimatedEndTime();
    }

    private Break CalculateBreakInterval()
    {
        if (previousActiveInterval == null || currentActiveInterval == null)
            return null;

        TimeSpan breakStartTime = previousActiveInterval.EndTime;
        TimeSpan breakEndTime = currentActiveInterval.StartTime;

        return new Break(breakStartTime, breakEndTime);
    }

    private void Clear()
    {
        AllIntervals.Clear();

        EstimatedEndTime = null;
        OverallBeginTime = null;
        OverallEndTime = null;
        TotalBreakTime = TimeSpan.Zero;

        LunchBreakTime = null;

        previousActiveInterval = null;
        currentActiveInterval = null;
    }

    private void UpdateBeginTime()
    {
        if (OverallBeginTime == null || currentActiveInterval.StartTime < OverallBeginTime)
            OverallBeginTime = currentActiveInterval.StartTime;
    }

    private void UpdateEndTime()
    {
        if (OverallEndTime == null || currentActiveInterval.EndTime > OverallEndTime)
            OverallEndTime = currentActiveInterval.EndTime;
    }

    private void UpdateActiveTime()
    {
        TotalActiveTime += currentActiveInterval.Interval;
    }

    private void UpdateBreakTime()
    {
        if (previousActiveInterval == null)
            return;

        TimeSpan breakStartTime = previousBreakInterval.StartTime;
        TimeSpan breakEndTime = previousBreakInterval.EndTime;
        TimeSpan breakTime = previousBreakInterval.Interval;

        bool isLunchBreak = breakStartTime >= minLunchTimeStart &&
                            breakEndTime <= maxLunchTimeEnd &&
                            breakTime >= minLunchTimeInterval &&
                            (LunchBreakTime == null || breakTime > LunchBreakTime);

        if (isLunchBreak)
        {
            if (LunchBreakTime != null)
                TotalBreakTime += LunchBreakTime.Value;

            LunchBreakTime = breakTime;
        }
        else
        {
            TotalBreakTime += breakTime;
        }
    }

    private void UpdateAllIntervalList()
    {
        if (previousBreakInterval != null)
            AllIntervals.Add(previousBreakInterval);

        if (currentActiveInterval != null)
            AllIntervals.Add(currentActiveInterval);
    }

    private TimeSpan CalculateTotalTime()
    {
        if (OverallBeginTime == null || OverallEndTime == null)
            return TimeSpan.Zero;

        return OverallEndTime.Value - OverallBeginTime.Value;
    }

    private TimeSpan? CalculateEstimatedEndTime()
    {
        if (OverallBeginTime == null || OverallEndTime == null)
            return null;

        return OverallBeginTime +
               TimeSpan.FromHours(7) +
               (TotalBreakTime < TimeSpan.FromHours(1) ? TimeSpan.FromHours(1) : TotalBreakTime) +
               (LunchBreakTime ?? (OverallEndTime.Value <= maxLunchTimeEnd - minLunchTimeInterval ? TimeSpan.FromHours(1) : TimeSpan.Zero));
    }
}