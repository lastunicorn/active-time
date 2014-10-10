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
using System.Windows.Input;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Services;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;

namespace DustInTheWind.ActiveTime.ViewModels
{
    public class FrontViewModel : ViewModelBase, INavigationAware
    {
        private readonly IStatusInfoService statusInfoService;
        private readonly IRegionManager regionManager;
        private readonly IStateService stateService;

        private readonly ICurrentDayRecord currentDayRecord;

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

        private DayTimeInterval[] records;

        public DayTimeInterval[] Records
        {
            get { return records; }
            private set
            {
                records = value;
                NotifyPropertyChanged("Records");
            }
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

        public ICommand TimeRecordsCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        private readonly DayTimeInterval aaa = new DayTimeInterval(TimeSpan.Zero, TimeSpan.Zero);
        public DayTimeInterval AAA
        {
            get { return aaa; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrontViewModel"/> class.
        /// </summary>
        public FrontViewModel(IStatusInfoService statusInfoService, IRegionManager regionManager, IStateService stateService, ICurrentDayRecord currentDayRecord)
        {
            if (statusInfoService == null)
                throw new ArgumentNullException("statusInfoService");

            if (regionManager == null)
                throw new ArgumentNullException("regionManager");

            if (stateService == null)
                throw new ArgumentNullException("stateService");

            if (currentDayRecord == null)
                throw new ArgumentNullException("currentDayRecord");

            this.statusInfoService = statusInfoService;
            this.regionManager = regionManager;
            this.stateService = stateService;
            this.currentDayRecord = currentDayRecord;

            CommentsCommand = new DelegateCommand(OnCommentsCommandExecuted);
            TimeRecordsCommand = new DelegateCommand(OnTimeRecordsCommandExecuted);
            RefreshCommand = new DelegateCommand(OnRefreshCommandExecuted);
            DeleteCommand = new DelegateCommand<object>(OnDeleteCommandExecuted);

            stateService.CurrentDateChanged += HandleStateService_CurrentDateChanged;
            currentDayRecord.ValueChanged += HandleCurrentDayRecordChanged;
        }

        private void HandleCurrentDayRecordChanged(object sender, EventArgs eventArgs)
        {
            UpdateDisplayedData();
        }

        private void HandleStateService_CurrentDateChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("Date");
        }

        private void OnDeleteCommandExecuted(object item)
        {
        }

        private void OnRefreshCommandExecuted()
        {
            currentDayRecord.Update();
            statusInfoService.SetStatus("Refreshed.");
        }

        private void OnCommentsCommandExecuted()
        {
            if (stateService.CurrentDate == null)
                return;

            //ClearRegion(RegionNames.MainContentRegion);
            //ClearRegion(RegionNames.RecordsRegion);
            //regionManager.Regions.Remove(RegionNames.RecordsRegion);

            regionManager.RequestNavigate(RegionNames.RecordsRegion, ViewNames.CommentsView);
        }

        private void OnTimeRecordsCommandExecuted()
        {
            if (stateService.CurrentDate == null)
                return;

            //ClearRegion(RegionNames.MainContentRegion);
            //ClearRegion(RegionNames.RecordsRegion);
            //regionManager.Regions.Remove(RegionNames.RecordsRegion);

            regionManager.RequestNavigate(RegionNames.RecordsRegion, ViewNames.DayRecordsView);
        }

        private void ClearRegion(string regionName)
        {
            IRegion mainContentRegion = regionManager.Regions[regionName];
            IViewsCollection activeViewsInRegion = mainContentRegion.ActiveViews;

            foreach (object view in activeViewsInRegion)
            {
                mainContentRegion.Remove(view);
            }
        }

        private void UpdateDisplayedData()
        {
            UpdateRecords();
            UpdateActiveTime();
            UpdateTotalTime();
            UpdateBeginTime();
            UpdateEstimatedEndTime();
        }

        private void UpdateRecords()
        {
            DayRecord dayRecord = currentDayRecord.Value;

            Records = dayRecord != null
                ? dayRecord.GetTimeRecords(false)
                : null;
        }

        private void UpdateActiveTime()
        {
            DayRecord dayRecord = currentDayRecord.Value;

            ActiveTime = dayRecord != null
                ? dayRecord.GetTotalActiveTime()
                : TimeSpan.Zero;
        }

        private void UpdateTotalTime()
        {
            DayRecord dayRecord = currentDayRecord.Value;

            TotalTime = dayRecord != null
                ? dayRecord.GetTotalTime()
                : TimeSpan.Zero;
        }

        private void UpdateBeginTime()
        {
            DayRecord dayRecord = currentDayRecord.Value;

            BeginTime = dayRecord == null 
                ? TimeSpan.Zero 
                : (dayRecord.GetBeginTime() ?? TimeSpan.Zero);
        }

        private void UpdateEstimatedEndTime()
        {
            EstimatedEndTime = BeginTime + TimeSpan.FromHours(9);
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
