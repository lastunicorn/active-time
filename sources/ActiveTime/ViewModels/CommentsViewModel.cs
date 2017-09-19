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

namespace DustInTheWind.ActiveTime.ViewModels
{
    internal class CommentsViewModel : ViewModelBase
    {
        private readonly ICurrentDayComment currentDayComment;

        public ResetCommentCommand ResetCommand { get; }
        public SaveCommentCommand SaveCommand { get; }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged();
                currentDayComment.Comment = value;
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

            ResetCommand = new ResetCommentCommand(currentDayComment);
            SaveCommand = new SaveCommentCommand(currentDayComment);
            
            currentDayComment.CommentChanged += HandleCurrentDayCommentChanged;

            currentDayComment.Update();
        }

        private void HandleCurrentDayCommentChanged(object sender, EventArgs e)
        {
            Comment = currentDayComment.Comment;
        }
    }
}
