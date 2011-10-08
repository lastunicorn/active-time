using System;
using System.Collections.Generic;
using System.ComponentModel;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Persistence.Entities;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace DustInTheWind.ActiveTime.Main.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IRecorder recorder;
        private readonly IStatusInfoService statusInfoService;
        private readonly ITimeRecordRepository recordRepository;

        /// <summary>
        /// The Time in miliseconds after which the status Text will be reset to the default one.
        /// </summary>
        private const int STATUS_TIMEOUT = 5000;


        private DateTime? date;

        public DateTime? Date
        {
            get { return date; }
            set
            {
                date = value;
                NotifyPropertyChanged("Date");
                //OnDateChanged(EventArgs.Empty);
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
                UpdateActiveTime();
                UpdateTotalTime();
                UpdateBeginTime();
                UpdateEstimatedEndTime();
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

        private ICommand commentsCommand;
        public ICommand CommentsCommand
        {
            get { return commentsCommand; }
        }

        private ICommand refreshCommand;
        public ICommand RefreshCommand
        {
            get { return refreshCommand; }
        }

        public MainViewModel(IRecorder recorder, IStatusInfoService statusInfoService, ITimeRecordRepository recordRepository)
        {
            if (recorder == null)
                throw new ArgumentNullException("recorder");

            if (statusInfoService == null)
                throw new ArgumentNullException("statusInfoService");

            if (recordRepository == null)
                throw new ArgumentNullException("recordRepository");

            this.recorder = recorder;
            this.statusInfoService = statusInfoService;
            this.recordRepository = recordRepository;

            commentsCommand = new DelegateCommand(OnCommentsCommandExecuted);
            refreshCommand = new DelegateCommand(OnRefreshCommandExecuted);
            Date = DateTime.Today;

            recorder.Started += new EventHandler(recorder_Started);
            recorder.Stopped += new EventHandler(recorder_Stopped);
            recorder.Stamping += new EventHandler(recorder_Stamping);
            recorder.Stamped += new EventHandler(recorder_Stamped);

            statusInfoService.SetStatus("Alez", STATUS_TIMEOUT);
        }

        private void OnRefreshCommandExecuted()
        {
            UpdateModel();
            statusInfoService.SetStatus("Refreshed.", STATUS_TIMEOUT);
        }

        private void OnCommentsCommandExecuted()
        {
            if (Date != null)
            {
                //CommentsWindow window = new CommentsWindow(new DayCommentRepository(), datePicker1.SelectedDate.Value);

                //window.Owner = this;
                //window.ShowDialog();
            }
        }

        private void recorder_Started(object sender, EventArgs e)
        {
            UpdateModel();
            statusInfoService.SetStatus("Recorder started.", STATUS_TIMEOUT);
        }

        private void recorder_Stopped(object sender, EventArgs e)
        {
            UpdateModel();
            statusInfoService.SetStatus("Recorder stopped.", STATUS_TIMEOUT);
        }

        private void recorder_Stamping(object sender, EventArgs e)
        {
            statusInfoService.SetStatus("Updating the current record's time.", STATUS_TIMEOUT);
        }

        private void recorder_Stamped(object sender, EventArgs e)
        {
            statusInfoService.SetStatus("Current record's time has been updated.", STATUS_TIMEOUT);
            UpdateModel();
        }

        private void UpdateModel()
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
