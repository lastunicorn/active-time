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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.ShellNavigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;

namespace DustInTheWind.ActiveTime.MainModule.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IRecorder recorder;
        private readonly IStatusInfoService statusInfoService;
        private readonly ITimeRecordRepository recordRepository;
        private readonly IRegionManager regionManager;
        private readonly IShellNavigator navigator;

        private DateTime? date;

        public DateTime? Date
        {
            get { return date; }
            set
            {
                date = value;
                NotifyPropertyChanged("Date");
                UpdateDisplayedData();
            }
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

        private readonly ICommand commentsCommand;
        public ICommand CommentsCommand
        {
            get { return commentsCommand; }
        }

        private readonly ICommand refreshCommand;
        public ICommand RefreshCommand
        {
            get { return refreshCommand; }
        }

        private readonly ICommand startCommand;
        public ICommand StartCommand
        {
            get { return startCommand; }
        }

        private readonly ICommand stopCommand;
        public ICommand StopCommand
        {
            get { return refreshCommand; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="recorder"></param>
        /// <param name="statusInfoService"></param>
        /// <param name="recordRepository"></param>
        /// <param name="regionManager"></param>
        /// <param name="navigator"></param>
        public MainViewModel(IRecorder recorder, IStatusInfoService statusInfoService,
            ITimeRecordRepository recordRepository, IRegionManager regionManager, IShellNavigator navigator)
        {
            if (recorder == null)
                throw new ArgumentNullException("recorder");

            if (statusInfoService == null)
                throw new ArgumentNullException("statusInfoService");

            if (recordRepository == null)
                throw new ArgumentNullException("recordRepository");

            if (regionManager == null)
                throw new ArgumentNullException("regionManager");

            if (navigator == null)
                throw new ArgumentNullException("navigator");

            this.recorder = recorder;
            this.statusInfoService = statusInfoService;
            this.recordRepository = recordRepository;
            this.regionManager = regionManager;
            this.navigator = navigator;

            commentsCommand = new DelegateCommand(OnCommentsCommandExecuted);
            refreshCommand = new DelegateCommand(OnRefreshCommandExecuted);
            startCommand = new DelegateCommand(OnStartCommandExecuted);
            stopCommand = new DelegateCommand(OnStopCommandExecuted);

            recorder.Started += new EventHandler(recorder_Started);
            recorder.Stopped += new EventHandler(recorder_Stopped);
            recorder.Stamping += new EventHandler(recorder_Stamping);
            recorder.Stamped += new EventHandler(recorder_Stamped);

            Date = DateTime.Today;

            //UpdateDisplayedData();
        }

        private void OnStartCommandExecuted()
        {
            recorder.Start();
        }

        private void OnStopCommandExecuted()
        {
            recorder.Stop();
        }

        private void OnRefreshCommandExecuted()
        {
            UpdateDisplayedData();
            statusInfoService.SetStatus("Refreshed.");
        }

        private void OnCommentsCommandExecuted()
        {
            if (Date != null)
            {
                //Dictionary<string, object> parameters = new Dictionary<string, object>();
                //parameters.Add("Text", "alez");
                //navigator.Navigate("MessageShell", parameters);

                regionManager.RequestNavigate(RegionNames.MainContentRegion, ViewNames.CommentsView);

                //CommentsWindow window = new CommentsWindow(new DayCommentRepository(), datePicker1.SelectedDate.Value);

                //window.Owner = this;
                //window.ShowDialog();
            }
        }

        private void recorder_Started(object sender, EventArgs e)
        {
            UpdateDisplayedData();
            statusInfoService.SetStatus("Recorder started.");
        }

        private void recorder_Stopped(object sender, EventArgs e)
        {
            UpdateDisplayedData();
            statusInfoService.SetStatus("Recorder stopped.");
        }

        private void recorder_Stamping(object sender, EventArgs e)
        {
            statusInfoService.SetStatus("Updating the current record's time.");
        }

        private void recorder_Stamped(object sender, EventArgs e)
        {
            statusInfoService.SetStatus("Current record's time has been updated.");
            UpdateDisplayedData();
        }

        private void UpdateDisplayedData()
        {
            if (Date != null)
            {
                IList<TimeRecord> timeRecords = recordRepository.GetByDate(Date.Value);
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
            ActiveTime = dayRecord != null ? dayRecord.GetTotalActiveTime() : TimeSpan.Zero;
        }

        private void UpdateTotalTime()
        {
            TotalTime = dayRecord != null ? dayRecord.GetTotalTime() : TimeSpan.Zero;
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
