using System;
using System.ComponentModel;
using DustInTheWind.ActiveTime.Persistence.Entities;
using DustInTheWind.ActiveTime.Persistence.Repositories;

namespace DustInTheWind.ActiveTime.UI.ViewModels
{
    class CommentsViewModel : INotifyPropertyChanged
    {
        private IDayCommentRepository commentRepository;

        private DateTime date = DateTime.Today;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Date"));
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comment"));
            }
        }

        private bool commentTextWrap = true;
        public bool CommentTextWrap
        {
            get { return commentTextWrap; }
            set
            {
                commentTextWrap = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentTextWrap"));
            }
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        public CommentsViewModel(IDayCommentRepository dayCommentRepository)
        {
            if (dayCommentRepository == null)
                throw new ArgumentNullException("dayCommentRepository");

            this.commentRepository = dayCommentRepository;
        }

        public void WindowLoaded()
        {
            try
            {
                DayComment dayComment = commentRepository.GetByDate(date);
                Comment = dayComment == null ? string.Empty : dayComment.Comment;
            }
            catch
            {
                //view.DisplayError(ex);
            }

        }
    }
}
