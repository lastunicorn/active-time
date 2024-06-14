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
using DustInTheWind.ActiveTime.Application.CurrentDate.DecrementDate;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Ports.SystemAccess;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.CurrentDate.ChangeDate;

internal class ChangeDateUseCase : IRequestHandler<ChangeDateRequest>
{
    private readonly CurrentDay currentDay;
    private readonly ISystemClock systemClock;
    private readonly EventBus eventBus;

    public ChangeDateUseCase(CurrentDay currentDay, ISystemClock systemClock, EventBus eventBus)
    {
        this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
        this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task Handle(ChangeDateRequest request, CancellationToken cancellationToken)
    {
        currentDay.Date = request.Date ?? systemClock.GetCurrentDate();
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