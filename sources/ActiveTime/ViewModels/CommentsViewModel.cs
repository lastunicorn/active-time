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
using DustInTheWind.ActiveTime.Commands;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Services;

namespace DustInTheWind.ActiveTime.ViewModels
{
    public class CommentsViewModel : ViewModelBase
    {
        private readonly ICurrentDayComment currentDayComment;

        public CustomDelegateCommand ResetCommand { get; }
        public CustomDelegateCommand SaveCommand { get; }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public CommentsViewModel(IStateService stateService, ICurrentDayComment currentDayComment)
        {
            if (stateService == null) throw new ArgumentNullException(nameof(stateService));
            if (currentDayComment == null) throw new ArgumentNullException(nameof(currentDayComment));

            this.currentDayComment = currentDayComment;
            currentDayComment.ValueChanged += HandleCurrentDayCommentChanged;

            ResetCommand = new CustomDelegateCommand(OnResetCommandExecute);
            SaveCommand = new CustomDelegateCommand(OnSaveCommandExecute);

            currentDayComment.Update();
        }

        private void HandleCurrentDayCommentChanged(object sender, EventArgs e)
        {
            UpdateDisplayedData();
        }

        private void UpdateDisplayedData()
        {
            Comment = currentDayComment.Value?.Comment;

            RefreshButtonsState();
        }

        private void RefreshButtonsState()
        {
            DayComment dayComment = currentDayComment.Value;

            bool isDataUnsaved = (dayComment != null && dayComment.Comment != comment) ||
                (dayComment == null && !string.IsNullOrEmpty(comment));

            SaveCommand.IsEnabled = isDataUnsaved;
        }

        private void OnResetCommandExecute(object parameter)
        {
            UpdateDisplayedData();
        }

        private void OnSaveCommandExecute(object parameter)
        {
            SaveInternal();
        }

        private void SaveInternal()
        {
            DayComment dayComment = currentDayComment.Value;

            if (dayComment == null)
                return;

            dayComment.Comment = Comment;
            currentDayComment.Save();

            RefreshButtonsState();
        }
    }
}
