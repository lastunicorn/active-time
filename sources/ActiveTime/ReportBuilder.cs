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
using System.Text;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.Recording;

namespace DustInTheWind.ActiveTime
{
    internal class ReportBuilder
    {
        private readonly IDayCommentRepository dayCommentRepository;
        private readonly ITimeRecordRepository timeRecordRepository;

        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }

        public IEnumerable<DayComment> DayComments { get; private set; }
        public string Text { get; private set; }

        public ReportBuilder(IDayCommentRepository dayCommentRepository, ITimeRecordRepository timeRecordRepository)
        {
            if (dayCommentRepository == null) throw new ArgumentNullException(nameof(dayCommentRepository));
            if (timeRecordRepository == null) throw new ArgumentNullException(nameof(timeRecordRepository));

            this.dayCommentRepository = dayCommentRepository;
            this.timeRecordRepository = timeRecordRepository;
        }

        public string Build()
        {
            RetrieveComments();
            Stringify();

            return Text;
        }

        private void RetrieveComments()
        {
            DayComments = dayCommentRepository.GetByDate(FirstDay, LastDay);
        }

        private void Stringify()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Average hours per day: ");

            TimeSpan averageHours = CalculateHours();
            sb.AppendLine(averageHours.ToDefaultFormat());

            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine();

            foreach (DayComment dayComment in DayComments)
            {
                IEnumerable<TimeRecord> timeRecords = timeRecordRepository.GetByDate(dayComment.Date);
                DayRecord dayRecord = new DayRecord(timeRecords);

                sb.Append(dayComment.Date.ToDefaultFormat());

                sb.Append(" - active: ");

                TimeSpan totalActiveTime = dayRecord.GetTotalActiveTime();
                sb.Append(totalActiveTime.ToDefaultFormat());

                sb.Append(" [ ");

                TimeSpan? beginTime = dayRecord.GetBeginTime();
                sb.Append(beginTime.ToDefaultFormat());

                sb.Append(" - ");

                TimeSpan? endTime = dayRecord.GetEndTime();
                sb.Append(endTime.ToDefaultFormat());

                sb.Append(" ] ");

                sb.AppendLine();
                sb.Append(dayComment.Comment);
                sb.AppendLine();
                sb.AppendLine("--------------------------------------------------");
                sb.AppendLine();
            }

            Text = sb.ToString();
        }

        private TimeSpan CalculateHours()
        {
            DateTime date = FirstDay;
            TimeSpan totalTime = TimeSpan.Zero;
            int dayCount = 0;

            while (date <= LastDay)
            {
                IEnumerable<TimeRecord> timeRecords = timeRecordRepository.GetByDate(date);
                DayRecord dayRecord = new DayRecord(timeRecords);

                TimeSpan totalDayActiveTime = dayRecord.GetTotalActiveTime();

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