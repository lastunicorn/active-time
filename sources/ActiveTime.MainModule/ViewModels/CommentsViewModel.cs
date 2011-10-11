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

namespace DustInTheWind.ActiveTime.MainModule.ViewModels
{
    public class CommentsViewModel : ViewModelBase, INavigationAware
    {
        private readonly ICommentsService commentsService;
        private readonly IRegionManager regionManager;

        private readonly ButtonBarViewModel buttonBarViewModel;
        public ButtonBarViewModel ButtonBarViewModel
        {
            get { return buttonBarViewModel; }
        }

        private DateTime? date;
        public DateTime? Date
        {
            get { return date; }
            set
            {
                date = value;
                NotifyPropertyChanged("Date");
            }
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
        /// <param name="commentsService"></param>
        /// <param name="regionManager"></param>
        public CommentsViewModel(ICommentsService commentsService, IRegionManager regionManager)
        {
            if (commentsService == null)
                throw new ArgumentNullException("commentsService");

            if (regionManager == null)
                throw new ArgumentNullException("regionManager");

            this.commentsService = commentsService;
            this.regionManager = regionManager;

            buttonBarViewModel = new ButtonBarViewModel();
            buttonBarViewModel.ApplyButtonClicked += new EventHandler(buttonBarViewModel_ApplyButtonClicked);
            buttonBarViewModel.CancelButtonClicked += new EventHandler(buttonBarViewModel_CancelButtonClicked);
            buttonBarViewModel.SaveButtonClicked += new EventHandler(buttonBarViewModel_SaveButtonClicked);
            
            commentsService.RecordChanged += new EventHandler(commentsService_RecordChanged);
            commentsService.RetrieveRecord(DateTime.Today);
            RefreshDisplayedData();
        }

        void commentsService_RecordChanged(object sender, EventArgs e)
        {
            RefreshDisplayedData();
        }

        private void RefreshDisplayedData()
        {
            if (commentsService.Record == null)
            {
                Date = null;
                Comment = null;

                buttonBarViewModel.DataState = ButtonBarViewModel.ButtonBarDataState.NoData;
            }
            else
            {
                Date = commentsService.Record.Date;
                Comment = commentsService.Record.Comment;

                buttonBarViewModel.DataState = ButtonBarViewModel.ButtonBarDataState.SavedData;
            }
        }

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

        private void NavigateToMainView()
        {
            regionManager.RequestNavigate(RegionNames.MainContentRegion, ViewNames.MainView);
        }

        private void SaveInternal()
        {
            if (commentsService.Record == null)
            {
                if (Date == null) return;
                commentsService.Record = new DayComment { Date = Date.Value };
            }
            commentsService.Record.Comment = Comment;
            commentsService.SaveRecord();
            buttonBarViewModel.DataState = ButtonBarViewModel.ButtonBarDataState.SavedData;
        }

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
    }
}
