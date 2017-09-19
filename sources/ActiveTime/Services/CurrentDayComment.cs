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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Services;

namespace DustInTheWind.ActiveTime.Services
{
    public class CurrentDayComment : ICurrentDayComment
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IStateService stateService;
        private readonly ILogger logger;

        private DayComment value;
        private string comment;

        private DayComment Value
        {
            get { return value; }
            set
            {
                this.value = value;
                Comment = value?.Comment;

                OnValueChanged(EventArgs.Empty);
            }
        }

        public string Comment
        {
            get { return comment; }
            set
            {
                if (comment == value)
                    return;

                comment = value;
                OnCommentChanged();
            }
        }

        public bool IsCommentSaved => (value == null && comment == null) || (value != null && value.Comment == comment);

        public event EventHandler ValueChanged;
        public event EventHandler CommentChanged;

        public CurrentDayComment(IUnitOfWorkFactory unitOfWorkFactory, IStateService stateService, ILogger logger)
        {
            if (unitOfWorkFactory == null) throw new ArgumentNullException(nameof(unitOfWorkFactory));
            if (stateService == null) throw new ArgumentNullException(nameof(stateService));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            this.unitOfWorkFactory = unitOfWorkFactory;
            this.stateService = stateService;
            this.logger = logger;

            stateService.CurrentDateChanged += HandleCurrentDateChanged;
        }

        private void HandleCurrentDateChanged(object sender, EventArgs e)
        {
            UpdateValueFromRepository();
        }

        public void Update()
        {
            UpdateValueFromRepository();
        }

        private void UpdateValueFromRepository()
        {
            DateTime? currentDate = stateService.CurrentDate;

            if (currentDate != null)
            {
                using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateNew())
                {
                    IDayCommentRepository dayCommentRepository = unitOfWork.DayCommentRepository;

                    DayComment dayComment = dayCommentRepository.GetByDate(currentDate.Value);
                    Value = dayComment ?? new DayComment { Date = currentDate.Value };
                }
            }
            else
            {
                Value = null;
            }
        }

        public void Save()
        {
            if (Value == null)
                return;

            Value.Comment = comment;

            logger.Log(Value);

            using (IUnitOfWork unitOfWork = unitOfWorkFactory.CreateNew())
            {
                IDayCommentRepository dayCommentRepository = unitOfWork.DayCommentRepository;

                dayCommentRepository.AddOrUpdate(Value);
                unitOfWork.Commit();
            }

            OnCommentChanged();
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        protected virtual void OnCommentChanged()
        {
            CommentChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}