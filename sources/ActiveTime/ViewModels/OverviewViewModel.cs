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
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Persistence;
using DustInTheWind.ActiveTime.Recording;
using DustInTheWind.ActiveTime.Services;
using Microsoft.Practices.ObjectBuilder2;

namespace DustInTheWind.ActiveTime.ViewModels
{
    public sealed class OverviewViewModel : ViewModelBase
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private string comments;
        public string Comments
        {
            get { return comments; }
            private set
            {
                comments = value;
                OnPropertyChanged();
            }
        }

        public List<DayReport> Reports { get; set; }

        private DateTime firstDay;
        public DateTime FirstDay
        {
            get { return firstDay; }
            set
            {
                firstDay = value;
                OnPropertyChanged();
                PopulateComments();
            }
        }

        private DateTime lastDay;
        public DateTime LastDay
        {
            get { return lastDay; }
            set
            {
                lastDay = value;
                OnPropertyChanged();
                PopulateComments();
            }
        }

        public OverviewViewModel(IUnitOfWorkFactory unitOfWorkFactory, ITimeProvider timeProvider)
        {
            if (unitOfWorkFactory == null) throw new ArgumentNullException(nameof(unitOfWorkFactory));
            if (timeProvider == null) throw new ArgumentNullException(nameof(timeProvider));

            this.unitOfWorkFactory = unitOfWorkFactory;

            DateTime today = timeProvider.GetDate();
            firstDay = today.AddDays(-29);
            lastDay = today;

            PopulateComments();
        }

        private void PopulateComments()
        {
            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateNew())
            {
                IDayCommentRepository dayCommentRepository = unitOfWork.DayCommentRepository;
                ITimeRecordRepository timeRecordRepository = unitOfWork.TimeRecordRepository;

                IEnumerable<DayComment> dayComments = dayCommentRepository.GetByDate(FirstDay, LastDay);
                Comments = Stringify(dayComments, timeRecordRepository);

                Reports = new List<DayReport>();
                dayComments.ForEach(x => Reports.Add(new DayReport(x)));
            }
        }

        private string Stringify(IEnumerable<DayComment> dayComments, ITimeRecordRepository timeRecordRepository)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Average hours per day: ");
            sb.AppendLine(CalculateHours(timeRecordRepository).ToString());

            foreach (DayComment dayComment in dayComments)
            {
                IEnumerable<TimeRecord> timeRecords = timeRecordRepository.GetByDate(dayComment.Date);
                DayRecord dayRecord = new DayRecord(timeRecords);

                sb.Append(dayComment.Date.ToShortDateString());

                sb.Append(" - active: ");
                sb.Append(dayRecord.GetTotalActiveTime());
                sb.Append(" [ ");
                sb.Append(dayRecord.GetBeginTime());
                sb.Append(" - ");
                sb.Append(dayRecord.GetEndTime());
                sb.Append(" ] ");

                sb.AppendLine();
                sb.Append(dayComment.Comment);
                sb.AppendLine();
                sb.AppendLine("--------------------------------------------------");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private TimeSpan CalculateHours(ITimeRecordRepository timeRecordRepository)
        {
            DateTime date = firstDay;
            TimeSpan totalTime = TimeSpan.Zero;
            int dayCount = 0;

            while (date <= lastDay)
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
