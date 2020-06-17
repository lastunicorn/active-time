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
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Presentation.Commands;
using DustInTheWind.ActiveTime.Presentation.Services;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public class CommentsViewModel : ViewModelBase
    {
        private readonly CurrentDay currentDay;

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                comment = value;
                OnPropertyChanged();
                currentDay.Comment = value;
            }
        }

        private bool commentTextWrap = true;
        public bool CommentTextWrap
        {
            get => commentTextWrap;
            set
            {
                commentTextWrap = value;
                OnPropertyChanged();
            }
        }

        public ResetCommentCommand ResetCommand { get; }
        public SaveCommentCommand SaveCommand { get; }

        public CommentsViewModel(CurrentDay currentDay)
        {
            this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));

            ResetCommand = new ResetCommentCommand(currentDay);
            SaveCommand = new SaveCommentCommand(currentDay);
            
            currentDay.CommentChanged += HandleCurrentDayCommentChanged;

            Comment = currentDay.Comment;

            currentDay.ReloadComments();
        }

        private void HandleCurrentDayCommentChanged(object sender, EventArgs e)
        {
            Comment = currentDay.Comment;
        }
    }
}
