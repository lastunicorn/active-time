using System;
using DustInTheWind.ActiveTime.Application;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public class TimeReportViewModel : ViewModelBase
    {
        private readonly CurrentDay currentDay;

        private TimeSpan activeTime;

        public TimeSpan ActiveTime
        {
            get => activeTime;
            private set
            {
                activeTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan totalTime;

        public TimeSpan TotalTime
        {
            get => totalTime;
            private set
            {
                totalTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan? beginTime;

        public TimeSpan? BeginTime
        {
            get => beginTime;
            set
            {
                beginTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan? estimatedEndTime;

        public TimeSpan? EstimatedEndTime
        {
            get => estimatedEndTime;
            set
            {
                estimatedEndTime = value;
                OnPropertyChanged();
            }
        }

        public TimeReportViewModel(CurrentDay currentDay)
        {
            this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
            
            currentDay.DatesChanged += HandleCurrentDayDatesChanged;
        }

        private void HandleCurrentDayDatesChanged(object sender, EventArgs eventArgs)
        {
            UpdateDisplayedData();
        }

        private void UpdateDisplayedData()
        {
            ActiveTime = currentDay.ActiveTime;
            TotalTime = currentDay.TotalTime;
            BeginTime = currentDay.BeginTime;
            EstimatedEndTime = currentDay.EstimatedEndTime;
        }
    }
}