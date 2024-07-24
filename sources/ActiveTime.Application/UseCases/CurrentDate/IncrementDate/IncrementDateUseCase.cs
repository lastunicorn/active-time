// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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
using DustInTheWind.ActiveTime.Application.UseCases.CurrentDate.DecrementDate;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.CurrentDate.IncrementDate;

internal class IncrementDateUseCase : IRequestHandler<IncrementDateRequest>
{
    private readonly CurrentDay currentDay;
    private readonly EventBus eventBus;

    public IncrementDateUseCase(CurrentDay currentDay, EventBus eventBus)
    {
        this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task Handle(IncrementDateRequest request, CancellationToken cancellationToken)
    {
        DateTime currentDate = currentDay.Date;

        if (currentDate >= DateTime.MaxValue.Date)
            throw new ActiveTimeException("We are already at the end of time. Tomorrow does not exist.");

        currentDay.IncrementDate();
        await RaiseCurrentDateChangedEvent();
    }

    private async Task RaiseCurrentDateChangedEvent()
    {
        CurrentDateChangedEvent currentDateChangedEvent = new()
        {
            Date = currentDay.Date
        };
        await eventBus.Publish(currentDateChangedEvent);
    }
}