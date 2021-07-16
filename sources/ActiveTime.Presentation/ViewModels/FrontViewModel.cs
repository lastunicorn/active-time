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
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Presentation.Commands;
using Microsoft.Practices.Prism.Regions;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public class FrontViewModel : ViewModelBase, INavigationAware
    {
        private readonly CurrentDay currentDay;

        public CurrentDateViewModel CurrentDateViewModel { get; }

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

        public CommentsCommand CommentsCommand { get; }
        public TimeRecordsCommand TimeRecordsCommand { get; }
        public RefreshCommand RefreshCommand { get; }
        public DeleteCommand DeleteCommand { get; }
        public DecrementDayCommand DecrementDayCommand { get; }
        public IncrementDayCommand IncrementDayCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontViewModel"/> class.
        /// </summary>
        public FrontViewModel(IRegionManager regionManager, CurrentDay currentDay, CurrentDateViewModel currentDateViewModel)
        {
            if (regionManager == null) throw new ArgumentNullException(nameof(regionManager));

            this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
            CurrentDateViewModel = currentDateViewModel ?? throw new ArgumentNullException(nameof(currentDateViewModel));

            CommentsCommand = new CommentsCommand(regionManager);
            TimeRecordsCommand = new TimeRecordsCommand(regionManager);
            RefreshCommand = new RefreshCommand(currentDay);
            DeleteCommand = new DeleteCommand();
            DecrementDayCommand = new DecrementDayCommand(currentDay);
            IncrementDayCommand = new IncrementDayCommand(currentDay);

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