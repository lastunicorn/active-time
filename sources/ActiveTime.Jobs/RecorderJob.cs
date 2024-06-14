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
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.Recording.Stamp;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;

namespace DustInTheWind.ActiveTime.Jobs;

/// <summary>
/// Periodically calls the scribe to update the time of the current record in the database.
/// </summary>
public class RecorderJob : PeriodicalJob
{
    private readonly IRequestBus requestBus;

    public override string Id { get; } = "Recorder";

    public RecorderJob(IRequestBus requestBus, ITimer timer)
        : base(timer)
    {
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        RunInterval = TimeSpan.FromMinutes(1);
    }

    protected override async Task DoExecute()
    {
        StampRequest stampRequest = new();
        await requestBus.Send(stampRequest);
    }
}