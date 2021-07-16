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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Logging;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using DayRecord = DustInTheWind.ActiveTime.Common.DayRecord;

namespace DustInTheWind.ActiveTime.Application
{
    public class CurrentDay
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly ILogger logger;
        private readonly IStatusInfoService statusInfoService;

        private DateTime? date;
        private DayRecord dayRecord;
        private string comment;

        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                statusInfoService.SetStatus("Date changed.");

                UpdateCommentsFromRepository();
                UpdateDayRecordFromRepository();

                OnDateChanged(EventArgs.Empty);
            }
        }

        public string Comment
        {
            get => comment;
            set
            {
                if (comment == value)
                    return;

                comment = value;
                OnCommentChanged();
            }
        }

        public bool IsCommentSaved => (dayRecord == null && comment == null) || (dayRecord != null && dayRecord.Comment == comment);

        public DayTimeInterval[] Records { get; private set; }
        public TimeSpan ActiveTime { get; private set; }
        public TimeSpan TotalTime { get; private set; }
        public TimeSpan? BeginTime { get; private set; }
        public TimeSpan? EstimatedEndTime { get; private set; }

        public event EventHandler DateChanged;
        public event EventHandler CommentChanged;
        public event EventHandler DatesChanged;

        public CurrentDay(IUnitOfWorkFactory unitOfWorkFactory, ILogger logger, IRecorderService recorder, IStatusInfoService statusInfoService)
        {
            if (recorder == null) throw new ArgumentNullException(nameof(recorder));

            this.unitOfWorkFactory = unitOfWorkFactory ?? throw new ArgumentNullException(nameof(unitOfWorkFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));

            date = DateTime.Today;

            recorder.Started += HandleRecorderStarted;
            recorder.Stopped += HandleRecorderStopped;
            recorder.Stamping += HandleRecorderStamping;
            recorder.Stamped += HandleRecorderStamped;
        }

        private void HandleRecorderStarted(object sender, EventArgs e)
        {
            UpdateDayRecordFromRepository();
            statusInfoService.SetStatus("Recorder started.");
        }

        private void HandleRecorderStopped(object sender, EventArgs e)
        {
            UpdateDayRecordFromRepository();
            statusInfoService.SetStatus("Recorder stopped.");
        }

        private void HandleRecorderStamping(object sender, EventArgs e)
        {
            statusInfoService.SetStatus("Updating the current record's time.");
        }

        private void HandleRecorderStamped(object sender, EventArgs e)
        {
            statusInfoService.SetStatus("Current record's time has been updated.");
            UpdateDayRecordFromRepository();
        }

        public void ReloadComments()
        {
            UpdateCommentsFromRepository();
        }

        public void ReloadDayRecord()
        {
            UpdateDayRecordFromRepository();
            statusInfoService.SetStatus("Refreshed.");
        }

        private void UpdateCommentsFromRepository()
        {
            if (date == null)
            {
                dayRecord = null;
                Comment = null;
                return;
            }

            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateNew())
            {
                IDayCommentRepository dayCommentRepository = unitOfWork.DayCommentRepository;

                dayRecord = dayCommentRepository.GetByDate(date.Value)
                             ?? new DayRecord { Date = date.Value };

                Comment = dayRecord?.Comment;
            }
        }

        private void UpdateDayRecordFromRepository()
        {
            if (date == null)
            {
                ClearDates();
                return;
            }

            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateNew())
            {
                ITimeRecordRepository timeRecordRepository = unitOfWork.TimeRecordRepository;

                IEnumerable<TimeRecord> timeRecords = timeRecordRepository.GetByDate(date.Value);
                SetDates(timeRecords);
            }
        }

        private void ClearDates()
        {
            Records = new DayTimeInterval[0];
            ActiveTime = TimeSpan.Zero;
            TotalTime = TimeSpan.Zero;
            BeginTime = TimeSpan.Zero;
            EstimatedEndTime = TimeSpan.Zero;

            OnDatesChanged();
        }

        private void SetDates(IEnumerable<TimeRecord> timeRecords)
        {
            Common.Recording.DayRecord dayRecord = new Common.Recording.DayRecord(timeRecords);

            Records = dayRecord.AllIntervals.ToArray();
            ActiveTime = dayRecord.TotalActiveTime;
            TotalTime = dayRecord.TotalTime;
            BeginTime = dayRecord.OverallBeginTime ?? TimeSpan.Zero;
            EstimatedEndTime = dayRecord.EstimatedEndTime ?? TimeSpan.Zero;

            OnDatesChanged();
        }

        public void SaveComments()
        {
            if (dayRecord == null)
                return;

            dayRecord.Comment = comment;

            logger.Log(dayRecord);

            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateNew())
            {
                IDayCommentRepository dayCommentRepository = unitOfWork.DayCommentRepository;

                dayCommentRepository.AddOrUpdate(dayRecord);
                unitOfWork.Commit();
            }

            OnCommentChanged();
        }

        protected virtual void OnDateChanged(EventArgs e)
        {
            DateChanged?.Invoke(this, e);
        }

        protected virtual void OnCommentChanged()
        {
            CommentChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDatesChanged()
        {
            DatesChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}