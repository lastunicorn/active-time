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
using DustInTheWind.ActiveTime.Commands;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Services;
using Microsoft.Practices.Prism.Regions;

namespace DustInTheWind.ActiveTime.ViewModels
{
    internal class FrontViewModel : ViewModelBase, INavigationAware
    {
        private readonly IStateService stateService;
        private readonly ICurrentDay currentDay;

        private DateTime? date;

        public DateTime? Date
        {
            get { return date; }
            set
            {
                if (date == value)
                    return;

                date = value;
                OnPropertyChanged();

                stateService.CurrentDate = value;
            }
        }

        private TimeSpan activeTime;

        public TimeSpan ActiveTime
        {
            get { return activeTime; }
            private set
            {
                activeTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan totalTime;

        public TimeSpan TotalTime
        {
            get { return totalTime; }
            private set
            {
                totalTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan? beginTime;

        public TimeSpan? BeginTime
        {
            get { return beginTime; }
            set
            {
                beginTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan? estimatedEndTime;

        public TimeSpan? EstimatedEndTime
        {
            get { return estimatedEndTime; }
            set
            {
                estimatedEndTime = value;
                OnPropertyChanged();
            }
        }

        public CommentsCommand CommentsCommand { get; }
        public TimeRecordsCommand TimeRecordsCommand { get; }
        public RefreshCommand RefreshCommand { get; }
        public DeleteCommand DeleteCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontViewModel"/> class.
        /// </summary>
        public FrontViewModel(IStatusInfoService statusInfoService, IRegionManager regionManager, IStateService stateService, ICurrentDay currentDay)
        {
            if (statusInfoService == null) throw new ArgumentNullException(nameof(statusInfoService));
            if (regionManager == null) throw new ArgumentNullException(nameof(regionManager));
            if (stateService == null) throw new ArgumentNullException(nameof(stateService));
            if (currentDay == null) throw new ArgumentNullException(nameof(currentDay));

            this.stateService = stateService;
            this.currentDay = currentDay;

            CommentsCommand = new CommentsCommand(regionManager, stateService);
            TimeRecordsCommand = new TimeRecordsCommand(regionManager, stateService);
            RefreshCommand = new RefreshCommand(statusInfoService, currentDay);
            DeleteCommand = new DeleteCommand();

            Date = stateService.CurrentDate;

            stateService.CurrentDateChanged += HandleStateService_CurrentDateChanged;
            currentDay.DatesChanged += HandleCurrentDayDatesChanged;
        }

        private void HandleCurrentDayDatesChanged(object sender, EventArgs eventArgs)
        {
            UpdateDisplayedData();
        }

        private void HandleStateService_CurrentDateChanged(object sender, EventArgs e)
        {
            Date = stateService.CurrentDate;
        }

        private void UpdateDisplayedData()
        {
            ActiveTime = currentDay.ActiveTime;
            TotalTime = currentDay.TotalTime;
            BeginTime = currentDay.BeginTime;
            EstimatedEndTime = currentDay.EstimatedEndTime;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}