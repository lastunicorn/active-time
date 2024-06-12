﻿// ActiveTime
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
using DustInTheWind.ActiveTime.Common.ApplicationStatuses;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Miscellaneous.Refresh;

internal class RefreshUseCase : IRequestHandler<RefreshRequest>
{
    private readonly EventBus eventBus;
    private readonly StatusInfoService statusInfoService;

    public RefreshUseCase(EventBus eventBus, StatusInfoService statusInfoService)
    {
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));
    }

    public async Task Handle(RefreshRequest request, CancellationToken cancellationToken)
    {
        await RaiseCurrentDateChangedEvent();
        UpdateApplicationStatus();
    }

    private async Task RaiseCurrentDateChangedEvent()
    {
        CurrentDateChangedEvent currentDateChangedEvent = new();
        await eventBus.Publish(currentDateChangedEvent);
    }

    private void UpdateApplicationStatus()
    {
        RefreshedStatus status = ApplicationStatus.Create<RefreshedStatus>();
        statusInfoService.SetStatus(status);
    }
}