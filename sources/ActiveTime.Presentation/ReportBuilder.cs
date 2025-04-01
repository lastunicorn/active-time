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
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Application.Recording;
using DustInTheWind.ActiveTime.Domain;

namespace DustInTheWind.ActiveTime.Presentation
{
    internal class ReportBuilder
    {
        public DateTime FirstDay { get; set; }

        public DateTime LastDay { get; set; }

        public List<DateRecord> DayRecords { get; set; }

        public string Build()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Average hours per day: ");

            TimeSpan averageHours = CalculateHours();
            sb.AppendLine(averageHours.ToDefaultFormat());

            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine();

            if (DayRecords != null)
            {
                foreach (DateRecord dayComment in DayRecords)
                {
                    DayRecord dayRecord = new DayRecord(dayComment.TimeRecords);

                    sb.Append(dayComment.Date.ToDefaultFormat());

                    sb.Append(" - active: ");

                    TimeSpan totalActiveTime = dayRecord.TotalActiveTime;
                    sb.Append(totalActiveTime.ToDefaultFormat());

                    sb.Append(" [ ");

                    TimeSpan? beginTime = dayRecord.OverallBeginTime;
                    sb.Append(beginTime.ToDefaultFormat());

                    sb.Append(" - ");

                    TimeSpan? endTime = dayRecord.OverallEndTime;
                    sb.Append(endTime.ToDefaultFormat());

                    sb.Append(" ] ");

                    sb.AppendLine();
                    sb.Append(dayComment.Comment);
                    sb.AppendLine();
                    sb.AppendLine("--------------------------------------------------");
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private TimeSpan CalculateHours()
        {
            DateTime date = FirstDay;
            TimeSpan totalTime = TimeSpan.Zero;
            int dayCount = 0;

            while (date <= LastDay)
            {
                DateRecord dateRec = DayRecords?.FirstOrDefault(x => x.Date == date);
                List<TimeRecord> timeRecords = dateRec?.TimeRecords ?? new List<TimeRecord>();

                DayRecord dayRecord = new DayRecord(timeRecords);

                TimeSpan totalDayActiveTime = dayRecord.TotalActiveTime;

                totalTime += totalDayActiveTime;

                double pauseTime = (totalDayActiveTime.TotalMinutes / 52.5) * 7.5;
                totalTime += TimeSpan.FromMinutes(pauseTime);

                dayCount++;

                date = date.AddDays(1);
            }

            double average = dayCount == 0
                ? 0
                : totalTime.TotalHours / dayCount;

            return TimeSpan.FromHours(average);
        }
    }
}