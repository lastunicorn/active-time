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
using System.ComponentModel;
using DustInTheWind.ActiveTime.Common.Entities;
using DustInTheWind.ActiveTime.Common;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace DustInTheWind.ActiveTime.MainModule.ViewModels
{
    class CommentsViewModel : ViewModelBase
    {
        private IDayCommentRepository dayCommentRepository;

        private DateTime date = DateTime.Today;
        public DateTime Date
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

        private ICommand applyCommand;
        public ICommand ApplyCommand
        {
            get { return applyCommand; }
        }

        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get { return cancelCommand; }
        }

        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get { return saveCommand; }
        }

        public CommentsViewModel(IDayCommentRepository dayCommentRepository)
        {
            if (dayCommentRepository == null)
                throw new ArgumentNullException("dayCommentRepository");

            this.dayCommentRepository = dayCommentRepository;
            applyCommand = new DelegateCommand(new Action(OnApplyCommandExecuted));
            cancelCommand = new DelegateCommand(new Action(OnCancelCommandExecuted));
            cancelCommand = new DelegateCommand(new Action(OnSaveCommandExecuted));
        }

        private void OnApplyCommandExecuted()
        {

        }

        private void OnCancelCommandExecuted()
        {

        }

        private void OnSaveCommandExecuted()
        {

        }

        public void WindowLoaded()
        {
            DayComment dayComment = dayCommentRepository.GetByDate(date);
            Comment = dayComment == null ? string.Empty : dayComment.Comment;
        }
    }
}
