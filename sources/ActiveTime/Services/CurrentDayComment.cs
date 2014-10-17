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

namespace DustInTheWind.ActiveTime.Services
{
    public class CurrentDayComment : ICurrentDayComment
    {
        private readonly IDayCommentRepository dayCommentRepository;
        private readonly IStateService stateService;

        private DayComment value;
        public DayComment Value
        {
            get { return value; }
            private set
            {
                this.value = value;
                OnValueChanged(EventArgs.Empty);
            }
        }

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged(EventArgs e)
        {
            EventHandler handler = ValueChanged;

            if (handler != null)
                handler(this, e);
        }

        public CurrentDayComment(IDayCommentRepository dayCommentRepository, IStateService stateService)
        {
            if (dayCommentRepository == null)
                throw new ArgumentNullException("dayCommentRepository");

            if (stateService == null)
                throw new ArgumentNullException("stateService");

            this.dayCommentRepository = dayCommentRepository;
            this.stateService = stateService;

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
                DayComment dayComment = dayCommentRepository.GetByDate(currentDate.Value);
                Value = dayComment ?? new DayComment { Date = currentDate.Value };
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

            dayCommentRepository.AddOrUpdate(Value);
        }
    }
}