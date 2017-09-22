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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;

namespace DustInTheWind.ActiveTime.Services
{
    public class CurrentDay : ICurrentDay
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly ILogger logger;
        private readonly IStatusInfoService statusInfoService;

        private DateTime? date;
        private DayComment dayComment;
        private DayRecord dayRecord;
        private string comment;

        public DateTime? Date
        {
            get { return date; }
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
            get { return comment; }
            set
            {
                if (comment == value)
                    return;

                comment = value;
                OnCommentChanged();
            }
        }

        public bool IsCommentSaved => (dayComment == null && comment == null) || (dayComment != null && dayComment.Comment == comment);

        public DayTimeInterval[] Records => dayRecord?.GetTimeRecords(true);
        public TimeSpan ActiveTime => dayRecord?.GetTotalActiveTime() ?? TimeSpan.Zero;
        public TimeSpan TotalTime => dayRecord?.GetTotalTime() ?? TimeSpan.Zero;
        public TimeSpan? BeginTime => dayRecord?.GetBeginTime() ?? TimeSpan.Zero;
        public TimeSpan? EstimatedEndTime => dayRecord?.GetEstimatedEndTime() ?? TimeSpan.Zero;

        public event EventHandler DateChanged;
        public event EventHandler CommentChanged;
        public event EventHandler DatesChanged;

        public CurrentDay(IUnitOfWorkFactory unitOfWorkFactory, ILogger logger, IRecorderService recorder, IStatusInfoService statusInfoService)
        {
            if (unitOfWorkFactory == null) throw new ArgumentNullException(nameof(unitOfWorkFactory));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (recorder == null) throw new ArgumentNullException(nameof(recorder));
            if (statusInfoService == null) throw new ArgumentNullException(nameof(statusInfoService));

            this.unitOfWorkFactory = unitOfWorkFactory;
            this.logger = logger;
            this.statusInfoService = statusInfoService;

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
                dayComment = null;
                Comment = null;
                return;
            }

            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateNew())
            {
                IDayCommentRepository dayCommentRepository = unitOfWork.DayCommentRepository;

                dayComment = dayCommentRepository.GetByDate(date.Value)
                             ?? new DayComment { Date = date.Value };

                Comment = dayComment?.Comment;
            }
        }

        private void UpdateDayRecordFromRepository()
        {
            if (date == null)
            {
                dayRecord = null;
                OnDatesChanged();
                return;
            }

            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateNew())
            {
                ITimeRecordRepository timeRecordRepository = unitOfWork.TimeRecordRepository;

                IEnumerable<TimeRecord> timeRecords = timeRecordRepository.GetByDate(date.Value);
                dayRecord = new DayRecord(timeRecords);

                OnDatesChanged();
            }
        }

        public void SaveComments()
        {
            if (dayComment == null)
                return;

            dayComment.Comment = comment;

            logger.Log(dayComment);

            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateNew())
            {
                IDayCommentRepository dayCommentRepository = unitOfWork.DayCommentRepository;

                dayCommentRepository.AddOrUpdate(dayComment);
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