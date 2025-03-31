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
using DustInTheWind.ActiveTime.Application.CurrentDate.IncrementDate;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.CurrentDate.DecrementDate
{
    internal class DecrementDateUseCase : IRequestHandler<IncrementDateRequest>
    {
        private readonly InMemoryState inMemoryState;
        private readonly EventBus eventBus;

        public DecrementDateUseCase(InMemoryState inMemoryState, EventBus eventBus)
        {
            this.inMemoryState = inMemoryState ?? throw new ArgumentNullException(nameof(inMemoryState));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public Task Handle(IncrementDateRequest request, CancellationToken cancellationToken)
        {
            DateTime currentDate = inMemoryState.CurrentDate;

            if (currentDate <= DateTime.MinValue.Date)
                throw new ActiveTimeException("This is already the first day of the known universe. There is no yesterday.");

            inMemoryState.CurrentDate = currentDate.Date.AddDays(-1);
            eventBus.Raise(EventNames.CurrentDate.CurrentDateChanged);

            return Task.FromResult(Unit.Value);
        }
    }
}