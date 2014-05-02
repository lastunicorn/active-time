// ActiveTime
// Copyright (C) 2011 Dust in the Wind
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
using System.Windows.Input;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;

namespace DustInTheWind.ActiveTime.MainGuiModule.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IStatusInfoService statusInfoService;
        private readonly ITimeRecordRepository timeRecordRepository;
        private readonly IRegionManager regionManager;
        private readonly IStateService stateService;

        public DateTime? Date
        {
            get { return stateService.CurrentDate; }
            set { stateService.CurrentDate = value; }
        }

        private TimeSpan activeTime;

        public TimeSpan ActiveTime
        {
            get { return activeTime; }
            private set
            {
                activeTime = value;
                NotifyPropertyChanged("ActiveTime");
            }
        }

        private TimeSpan totalTime;

        public TimeSpan TotalTime
        {
            get { return totalTime; }
            private set
            {
                totalTime = value;
                NotifyPropertyChanged("TotalTime");
            }
        }


        private DayRecord dayRecord;

        public DayRecord DayRecord
        {
            get { return dayRecord; }
            set
            {
                dayRecord = value;
                NotifyPropertyChanged("DayRecord");
                NotifyPropertyChanged("Records");
            }
        }

        public DayTimeInterval[] Records
        {
            get { return dayRecord == null ? null : dayRecord.GetTimeRecords(false); }
        }

        private TimeSpan? beginTime;

        public TimeSpan? BeginTime
        {
            get { return beginTime; }
            set
            {
                beginTime = value;
                NotifyPropertyChanged("BeginTime");
            }
        }

        private TimeSpan? estimatedEndTime;

        public TimeSpan? EstimatedEndTime
        {
            get { return estimatedEndTime; }
            set
            {
                estimatedEndTime = value;
                NotifyPropertyChanged("EstimatedEndTime");
            }
        }

        public ICommand CommentsCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        private DayTimeInterval aaa = new DayTimeInterval(TimeSpan.Zero, TimeSpan.Zero);
        public DayTimeInterval AAA
        {
            get { return aaa; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel(IRecorderService recorder, IStatusInfoService statusInfoService,
            ITimeRecordRepository timeRecordRepository, IRegionManager regionManager, IStateService stateService)
        {
            if (recorder == null)
                throw new ArgumentNullException("recorder");

            if (statusInfoService == null)
                throw new ArgumentNullException("statusInfoService");

            if (timeRecordRepository == null)
                throw new ArgumentNullException("timeRecordRepository");

            if (regionManager == null)
                throw new ArgumentNullException("regionManager");

            if (stateService == null)
                throw new ArgumentNullException("stateService");

            this.statusInfoService = statusInfoService;
            this.timeRecordRepository = timeRecordRepository;
            this.regionManager = regionManager;
            this.stateService = stateService;

            CommentsCommand = new DelegateCommand(OnCommentsCommandExecuted);
            RefreshCommand = new DelegateCommand(OnRefreshCommandExecuted);
            DeleteCommand = new DelegateCommand<object>(OnDeleteCommandExecuted);

            recorder.Started += HandleRecorderStarted;
            recorder.Stopped += HandleRecorderStopped;
            recorder.Stamping += HandleRecorderStamping;
            recorder.Stamped += HandleRecorderStamped;

            stateService.CurrentDateChanged += HandleStateService_CurrentDateChanged;
        }

        private void HandleStateService_CurrentDateChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("Date");
            UpdateDisplayedData();
        }

        private void OnDeleteCommandExecuted(object item)
        {
        }

        private void OnRefreshCommandExecuted()
        {
            UpdateDisplayedData();
            statusInfoService.SetStatus("Refreshed.");
        }

        private void OnCommentsCommandExecuted()
        {
            if (Date == null)
                return;

            regionManager.RequestNavigate(RegionNames.MainContentRegion, ViewNames.CommentsView);
        }

        private void HandleRecorderStarted(object sender, EventArgs e)
        {
            UpdateDisplayedData();
            statusInfoService.SetStatus("Recorder started.");
        }

        private void HandleRecorderStopped(object sender, EventArgs e)
        {
            UpdateDisplayedData();
            statusInfoService.SetStatus("Recorder stopped.");
        }

        private void HandleRecorderStamping(object sender, EventArgs e)
        {
            statusInfoService.SetStatus("Updating the current record's time.");
        }

        private void HandleRecorderStamped(object sender, EventArgs e)
        {
            statusInfoService.SetStatus("Current record's time has been updated.");
            UpdateDisplayedData();
        }

        private void UpdateDisplayedData()
        {
            if (Date != null)
            {
                IList<TimeRecord> timeRecords = timeRecordRepository.GetByDate(Date.Value);
                DayRecord dayRecord = DayRecord.FromTimeRecords(timeRecords);
                DayRecord = dayRecord ?? new DayRecord(Date.Value);
            }
            else
            {
                DayRecord = null;
            }

            UpdateActiveTime();
            UpdateTotalTime();
            UpdateBeginTime();
            UpdateEstimatedEndTime();
        }

        private void UpdateActiveTime()
        {
            ActiveTime = dayRecord != null
                ? dayRecord.GetTotalActiveTime()
                : TimeSpan.Zero;
        }

        private void UpdateTotalTime()
        {
            TotalTime = dayRecord != null
                ? dayRecord.GetTotalTime()
                : TimeSpan.Zero;
        }

        private void UpdateBeginTime()
        {
            BeginTime = dayRecord.GetBeginTime() ?? TimeSpan.Zero;
        }

        private void UpdateEstimatedEndTime()
        {
            EstimatedEndTime = BeginTime + TimeSpan.FromHours(9);
        }
    }
}
