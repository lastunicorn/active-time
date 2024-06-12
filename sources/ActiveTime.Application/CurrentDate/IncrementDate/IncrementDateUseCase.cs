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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.CurrentDate.IncrementDate
{
    internal class IncrementDateUseCase : IRequestHandler<IncrementDateRequest>
    {
        private readonly CurrentDay currentDay;
        private readonly EventBus eventBus;

        public IncrementDateUseCase(CurrentDay currentDay, EventBus eventBus)
        {
            this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public Task Handle(IncrementDateRequest request, CancellationToken cancellationToken)
        {
            DateTime currentDate = currentDay.Date;

            if (currentDate >= DateTime.MaxValue.Date)
                throw new ActiveTimeException("We are already at the end of time. Tomorrow does not exist.");

            currentDay.IncrementDate();
            RaiseCurrentDateChangedEvent();

            return Task.FromResult(Unit.Value);
        }

        private void RaiseCurrentDateChangedEvent()
        {
            EventParameters eventParameters = new EventParameters();
            eventParameters.Add("Date", currentDay.Date);

            eventBus.Raise(EventNames.CurrentDate.CurrentDateChanged, eventParameters);
        }
    }
}