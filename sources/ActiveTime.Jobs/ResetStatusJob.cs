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

using DustInTheWind.ActiveTime.Application.StatusManagement;
using DustInTheWind.ActiveTime.Application.UseCases.Miscellaneous.ResetStatus;
using DustInTheWind.ActiveTime.Infrastructure.JobEngine;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine;
using ITimer = DustInTheWind.ActiveTime.Infrastructure.JobEngine.ITimer;

namespace DustInTheWind.ActiveTime.Jobs;

public class ResetStatusJob : OneTimeJob
{
    private readonly IRequestBus requestBus;

    public override string Id { get; } = "ResetStatus";

    public ResetStatusJob(IRequestBus requestBus, ITimer timer, EventBus eventBus)
        : base(timer)
    {
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        eventBus.Subscribe<ApplicationStatusChangedEvent>(HandleApplicationStatusChanged);
    }

    private async Task HandleApplicationStatusChanged(ApplicationStatusChangedEvent arg1, CancellationToken arg2)
    {
        Timer.Interval = TimeSpan.FromSeconds(5);
        await Start();
    }

    protected override async Task DoExecute()
    {
        ResetStatusRequest stampRequest = new();
        await requestBus.Send(stampRequest);
    }
}