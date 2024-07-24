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
using DustInTheWind.ActiveTime.Infrastructure.JobEngine;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.Recording.PresentRecorderState;

internal class PresentRecorderStateUseCase : IRequestHandler<PresentRecorderStateRequest, PresentRecorderStateResponse>
{
    private readonly JobCollection jobCollection;

    public PresentRecorderStateUseCase(JobCollection jobCollection)
    {
        this.jobCollection = jobCollection ?? throw new ArgumentNullException(nameof(jobCollection));
    }

    public Task<PresentRecorderStateResponse> Handle(PresentRecorderStateRequest request, CancellationToken cancellationToken)
    {
        IJob recorderJob = jobCollection.Get(JobNames.Recorder);

        PresentRecorderStateResponse response = new()
        {
            IsRunning = recorderJob.State == JobState.Running
        };

        return Task.FromResult(response);
    }
}