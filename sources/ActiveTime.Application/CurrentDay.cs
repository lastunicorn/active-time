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
using DustInTheWind.ActiveTime.Infrastructure.EventModel;

namespace DustInTheWind.ActiveTime.Application
{
    public class CurrentDay
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly ILogger logger;
        private readonly IStatusInfoService statusInfoService;

        private DateTime? date;
        private DateRecord dateRecord;
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

        public bool IsCommentSaved => (dateRecord == null && comment == null) || (dateRecord != null && dateRecord.Comment == comment);

        public DayTimeInterval[] Records { get; private set; }
        
        public TimeSpan ActiveTime { get; private set; }
        
        public TimeSpan TotalTime { get; private set; }
        
        public TimeSpan? BeginTime { get; private set; }
        
        public TimeSpan? EstimatedEndTime { get; private set; }

        public event EventHandler DateChanged;
        public event EventHandler CommentChanged;
        public event EventHandler DatesChanged;

        public CurrentDay(IUnitOfWorkFactory unitOfWorkFactory, ILogger logger, EventBus eventBus, IStatusInfoService statusInfoService)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));

            this.unitOfWorkFactory = unitOfWorkFactory ?? throw new ArgumentNullException(nameof(unitOfWorkFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));

            date = DateTime.Today;

            eventBus.Subscribe(EventNames.Recorder.Started, HandleRecorderStarted);
            eventBus.Subscribe(EventNames.Recorder.Stopped, HandleRecorderStopped);
            eventBus.Subscribe(EventNames.Recorder.Stamped, HandleRecorderStamped);
        }

        private void HandleRecorderStarted(EventParameters parameters)
        {
            UpdateDayRecordFromRepository();
        }

        private void HandleRecorderStopped(EventParameters parameters)
        {
            UpdateDayRecordFromRepository();
        }

        private void HandleRecorderStamped(EventParameters parameters)
        {
            UpdateDayRecordFromRepository();
        }

        public void ReloadComments()
        {
            UpdateCommentsFromRepository();
        }

        public void ReloadDayRecord()
        {
            UpdateDayRecordFromRepository();
        }

        private void UpdateCommentsFromRepository()
        {
            if (date == null)
            {
                dateRecord = null;
                Comment = null;
                return;
            }

            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateNew())
            {
                IDateRecordRepository dateRecordRepository = unitOfWork.DateRecordRepository;

                dateRecord = dateRecordRepository.GetByDate(date.Value)
                             ?? new DateRecord { Date = date.Value };

                Comment = dateRecord?.Comment;
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
                IEnumerable<TimeRecord> timeRecords = unitOfWork.TimeRecordRepository.GetByDate(date.Value);
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
            if (dateRecord == null)
                return;

            dateRecord.Comment = comment;

            logger.Log(dateRecord);

            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateNew())
            {
                IDateRecordRepository dateRecordRepository = unitOfWork.DateRecordRepository;

                dateRecordRepository.AddOrUpdate(dateRecord);
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