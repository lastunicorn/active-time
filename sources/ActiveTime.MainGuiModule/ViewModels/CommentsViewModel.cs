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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;
using Microsoft.Practices.Prism.Regions;

namespace DustInTheWind.ActiveTime.MainGuiModule.ViewModels
{
    public class CommentsViewModel : ViewModelBase, INavigationAware
    {
        private readonly IStateService stateService;
        private readonly IRegionManager regionManager;
        private readonly IDayCommentRepository dayCommentRepository;

        private DayComment record;

        private readonly ButtonBarViewModel buttonBarViewModel;
        public ButtonBarViewModel ButtonBarViewModel
        {
            get { return buttonBarViewModel; }
        }

        public DateTime? Date
        {
            get { return stateService.CurrentDate; }
            set { stateService.CurrentDate = value; }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                NotifyPropertyChanged("Comment");
            }
        }

        private bool commentTextWrap = true;
        public bool CommentTextWrap
        {
            get { return commentTextWrap; }
            set
            {
                commentTextWrap = value;
                NotifyPropertyChanged("CommentTextWrap");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsViewModel"/> class.
        /// </summary>
        /// <param name="stateService"></param>
        /// <param name="regionManager"></param>
        public CommentsViewModel(IStateService stateService, IRegionManager regionManager, IDayCommentRepository dayCommentRepository)
        {
            if (stateService == null)
                throw new ArgumentNullException("stateService");

            if (regionManager == null)
                throw new ArgumentNullException("regionManager");

            if (dayCommentRepository == null)
                throw new ArgumentNullException("dayCommentRepository");

            this.stateService = stateService;
            this.regionManager = regionManager;
            this.dayCommentRepository = dayCommentRepository;

            buttonBarViewModel = new ButtonBarViewModel();
            buttonBarViewModel.ApplyButtonClicked += new EventHandler(buttonBarViewModel_ApplyButtonClicked);
            buttonBarViewModel.CancelButtonClicked += new EventHandler(buttonBarViewModel_CancelButtonClicked);
            buttonBarViewModel.SaveButtonClicked += new EventHandler(buttonBarViewModel_SaveButtonClicked);

            stateService.CurrentDateChanged += new EventHandler(commentsService_CurrentDateChanged);

            RetrieveRecord();
            RefreshDisplayedData();
        }

        void commentsService_CurrentDateChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("Date");

            RetrieveRecord();
            RefreshDisplayedData();
        }

        private void RefreshDisplayedData()
        {
            if (record == null)
            {
                Comment = null;
                buttonBarViewModel.DataState = ButtonBarViewModel.ButtonBarDataState.NoData;
            }
            else
            {
                Comment = record.Comment;
                buttonBarViewModel.DataState = ButtonBarViewModel.ButtonBarDataState.SavedData;
            }
        }

        private void RetrieveRecord()
        {
            DateTime? date = stateService.CurrentDate;

            if (date == null)
            {
                record = null;
            }
            else
            {
                record = dayCommentRepository.GetByDate(date.Value);
                if (record != null && record.Date != date)
                    record = null;
            }
        }

        #region Apply / Cancel / Save Buttons

        void buttonBarViewModel_ApplyButtonClicked(object sender, EventArgs e)
        {
            SaveInternal();
        }

        void buttonBarViewModel_CancelButtonClicked(object sender, EventArgs e)
        {
            NavigateToMainView();
        }

        void buttonBarViewModel_SaveButtonClicked(object sender, EventArgs e)
        {
            SaveInternal();
            NavigateToMainView();
        }

        #endregion

        private void NavigateToMainView()
        {
            regionManager.RequestNavigate(RegionNames.MainContentRegion, ViewNames.MainView);
        }

        private void SaveInternal()
        {
            DateTime? date = stateService.CurrentDate;

            if (record == null)
            {
                if (date == null) return;
                record = new DayComment { Date = date.Value };
            }
            record.Comment = Comment;
            dayCommentRepository.AddOrUpdate(record);
            buttonBarViewModel.DataState = ButtonBarViewModel.ButtonBarDataState.SavedData;
        }

        #region INavigationAware

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            string dateTicksAsString = navigationContext.Parameters["Date"];

            long dateTicks;

            if (long.TryParse(dateTicksAsString, out dateTicks))
            {
                Date = new DateTime(dateTicks);
            }
        }

        #endregion
    }
}
