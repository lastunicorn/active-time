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
using System.ComponentModel;
using DustInTheWind.ActiveTime.Recording;

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

        private TimeSpan activeTime;

        public TimeSpan ActiveTime
        {
            get { return activeTime; }
            private set
            {
                activeTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActiveTime"));
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


        private DayRecord dayRecord;

        public DayRecord DayRecord
        {
            get { return dayRecord; }
            set
            {
                dayRecord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DayRecord"));
                OnPropertyChanged(new PropertyChangedEventArgs("Records"));
                UpdateActiveTime();
                UpdateTotalTime();
                UpdateBeginTime();
                UpdateEstimatedEndTime();
            }
        }

        public Record[] Records
        {
            get { return dayRecord == null ? null : dayRecord.GetRecords(false); }
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

        private TimeSpan? beginTime;

        public TimeSpan? BeginTime
        {
            get { return beginTime; }
            set
            {
                beginTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BeginTime"));
            }
        }

        private TimeSpan? estimatedEndTime;

        public TimeSpan? EstimatedEndTime
        {
            get { return estimatedEndTime; }
            set
            {
                estimatedEndTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EstimatedEndTime"));
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
