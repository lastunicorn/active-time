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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.MainGuiModule.Commands;
using Microsoft.Practices.Prism.Regions;

namespace DustInTheWind.ActiveTime.MainGuiModule.ViewModels
{
    public class CommentsViewModel : ViewModelBase, INavigationAware
    {
        private readonly IStateService stateService;
        private readonly IRegionManager regionManager;
        private readonly IDayCommentRepository dayCommentRepository;

        private DayComment currentCommentRecord;

        public CustomDelegateCommand ApplyCommand { get; private set; }
        public CustomDelegateCommand CancelCommand { get; private set; }
        public CustomDelegateCommand SaveCommand { get; private set; }

        private DateTime? date;

        public DateTime? Date
        {
            get { return date; }
            set
            {
                SetDateInternal(value);
                stateService.CurrentDate = value;
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                SetCommentInternal(value);
                RefreshButtonsState();
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

            ApplyCommand = new CustomDelegateCommand(HandleApplyButtonClicked);
            CancelCommand = new CustomDelegateCommand(HandleCancelButtonClicked);
            SaveCommand = new CustomDelegateCommand(HandleSaveButtonClicked);

            Enable();
        }

        private void Enable()
        {
            stateService.CurrentDateChanged += HandleStateServiceCurrentDateChanged;
            PopulateView();
        }

        private void Disable()
        {
            stateService.CurrentDateChanged -= HandleStateServiceCurrentDateChanged;
        }

        private void HandleStateServiceCurrentDateChanged(object sender, EventArgs e)
        {
            PopulateView();
        }

        private void PopulateView()
        {
            SetDateInternal(stateService.CurrentDate);

            if (date == null)
            {
                currentCommentRecord = null;
                SetCommentInternal(null);
            }
            else
            {
                currentCommentRecord = dayCommentRepository.GetByDate(date.Value) ?? new DayComment { Date = date.Value };
                SetCommentInternal(currentCommentRecord.Comment);
            }

            RefreshButtonsState();
        }

        private void RefreshButtonsState()
        {
            bool isDataUnsaved = (currentCommentRecord != null && currentCommentRecord.Comment != comment) ||
                (currentCommentRecord == null && !string.IsNullOrEmpty(comment));

            ApplyCommand.IsEnabled = isDataUnsaved;
            SaveCommand.IsEnabled = isDataUnsaved;
        }

        private void HandleApplyButtonClicked(object parameter)
        {
            SaveInternal();
        }

        private void HandleCancelButtonClicked(object parameter)
        {
            NavigateToMainView();
        }

        private void HandleSaveButtonClicked(object parameter)
        {
            SaveInternal();
            NavigateToMainView();
        }

        private void SetDateInternal(DateTime? value)
        {
            date = value;
            NotifyPropertyChanged("Date");
        }

        private void SetCommentInternal(string value)
        {
            comment = value;
            NotifyPropertyChanged("Comment");
        }

        private void NavigateToMainView()
        {
            regionManager.RequestNavigate(RegionNames.MainContentRegion, ViewNames.MainView);
        }

        private void SaveInternal()
        {
            if (currentCommentRecord == null)
                return;

            currentCommentRecord.Comment = Comment;
            dayCommentRepository.AddOrUpdate(currentCommentRecord);

            RefreshButtonsState();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Enable();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Disable();
        }
    }
}
