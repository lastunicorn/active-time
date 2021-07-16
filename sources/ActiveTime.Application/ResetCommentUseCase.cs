using System;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;

namespace DustInTheWind.ActiveTime.Application
{
    internal class ResetCommentUseCase
    {
        private readonly ApplicationState applicationState;
        private readonly IUnitOfWork unitOfWork;

        public ResetCommentUseCase(ApplicationState applicationState, IUnitOfWork unitOfWork)
        {
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Execute()
        {

        }

        private void UpdateCommentsFromRepository()
        {
            DateTime? date = applicationState.CurrentDate;

            if (date == null)
            {
                applicationState.Comment = null;
                return;
            }

            IDayCommentRepository dayCommentRepository = unitOfWork.DayCommentRepository;

            dayComment = dayCommentRepository.GetByDate(date.Value) ?? new DayRecord { Date = date.Value };

            Comment = dayComment?.Comment;
        }
    }

    internal class ApplicationState
    {
        private DateTime? date;
        private string comment;

        public DateTime? CurrentDate
        {
            get => date;
            set
            {
                date = value;
                OnDateChanged();
            }
        }

        public string Comment
        {
            get => comment;
            set
            {
                if (comment == value)
                    return;

                comment = value;
                OnCommentChanged();
            }
        }

        public event EventHandler DateChanged;
        public event EventHandler CommentChanged;

        protected virtual void OnDateChanged()
        {
            DateChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCommentChanged()
        {
            CommentChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}