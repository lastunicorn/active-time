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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Presentation.ShellNavigation;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Presentation.Commands;
using MediatR;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public class CurrentDateViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private readonly CurrentDay currentDay;

        private DateTime? date;

        public DateTime? Date
        {
            get => date;
            set
            {
                if (date == value)
                    return;

                date = value;
                OnPropertyChanged();

                currentDay.Date = value;
            }
        }

        public CalendarCommand CalendarCommand { get; }

        public DecrementDayCommand DecrementDayCommand { get; }

        public IncrementDayCommand IncrementDayCommand { get; }

        public CurrentDateViewModel(IMediator mediator, EventBus eventBus, CurrentDay currentDay, IShellNavigator shellNavigator)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));

            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));

            CalendarCommand = new CalendarCommand(shellNavigator);
            DecrementDayCommand = new DecrementDayCommand(currentDay);
            IncrementDayCommand = new IncrementDayCommand(currentDay);

            Date = currentDay.Date;

            eventBus.Subscribe(EventNames.CurrentDate.CurrentDateChanged, HandleCurrentDateChanged);
        }

        private void HandleCurrentDateChanged(EventParameters parameters)
        {
            Date = parameters.Get<DateTime>("Date");
        }
    }
}