using System;
using System.ComponentModel;

namespace DustInTheWind.ActiveTime.UI.Models
{
    public class StatisticsModel : INotifyPropertyChanged
    {
        private Month[] months;

        public Month[] Months
        {
            get { return months; }
        }

        private int year;

        public int Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Year"));
            }
        }

        private Month selectedMonth;

        public Month SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedMonth"));
            }
        }

        private TimeSpan? totalWorkTime;

        public TimeSpan? TotalWorkTime
        {
            get { return totalWorkTime; }
            set
            {
                totalWorkTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalWorkTime"));
            }
        }

        private TimeSpan? totalBreakTime;

        public TimeSpan? TotalBreakTime
        {
            get { return totalBreakTime; }
            set
            {
                totalBreakTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalBreakTime"));
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

        public StatisticsModel()
        {
            months = new Month[12];

            for (int i = 0; i < 12; i++)
            {
                months[i] = new Month(i + 1, string.Format("{0:MMMM}", new DateTime(1, i + 1, 1)));
            }
        }
    }
}
