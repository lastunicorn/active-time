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
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.CurrentDate.DecrementDate;

internal class DecrementDateUseCase : IRequestHandler<DecrementDateRequest>
{
    private readonly CurrentDay currentDay;
    private readonly EventBus eventBus;

    public DecrementDateUseCase(CurrentDay currentDay, EventBus eventBus)
    {
        this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task Handle(DecrementDateRequest request, CancellationToken cancellationToken)
    {
        DateTime currentDate = currentDay.Date;

        if (currentDate <= DateTime.MinValue.Date)
            throw new ActiveTimeException("This is already the first day of the known universe. There is no yesterday.");

        currentDay.DecrementDate();
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