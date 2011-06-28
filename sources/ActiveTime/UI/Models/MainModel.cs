using System;
using System.ComponentModel;
using DustInTheWind.ActiveTime.Goose;

namespace DustInTheWind.ActiveTime.UI.Models
{
    internal class MainModel : INotifyPropertyChanged
    {
        private DateTime? date;

        public DateTime? Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Date"));
                OnDateChanged(EventArgs.Empty);
            }
        }

        private TimeSpan totalTime;

        public TimeSpan TotalTime
        {
            get { return totalTime; }
            private set
            {
                totalTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalTime"));
            }
        }

        private TimeSpan intervalTime;

        public TimeSpan IntervalTime
        {
            get { return intervalTime; }
            private set
            {
                intervalTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IntervalTime"));
            }
        }


        private DayRecord dayRecord;

        public DayRecord DayRecord
        {
            get { return dayRecord; }
            set
            {
                dayRecord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DayRecord"));
                OnPropertyChanged(new PropertyChangedEventArgs("Records"));
                UpdateTotalTime();
                UpdateFullTime();
            }
        }

        public Record[] Records
        {
            get { return dayRecord == null ? null : dayRecord.Records; }
        }


        private string statusText;

        public string StatusText
        {
            get { return statusText; }
            set
            {
                statusText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusText"));
            }
        }


        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        #region DateChanged

        public event EventHandler DateChanged;

        protected virtual void OnDateChanged(EventArgs e)
        {
            if (DateChanged != null)
                DateChanged(this, e);
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainModel"/> class.
        /// </summary>
        public MainModel()
        {
        }

        #endregion


        private void UpdateTotalTime()
        {
            if (dayRecord != null)
            {
                TotalTime = dayRecord.GetTotalTime();
            }
            else
            {
                TotalTime = TimeSpan.Zero;
            }
        }

        private void UpdateFullTime()
        {
            if (dayRecord != null)
            {
                IntervalTime = dayRecord.GetIntervalTime();
            }
            else
            {
                IntervalTime = TimeSpan.Zero;
            }
        }
    }
}
