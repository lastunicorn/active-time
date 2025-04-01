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
using DustInTheWind.ActiveTime.Application.Recording;
using DustInTheWind.ActiveTime.Application.StatusManagement;
using DustInTheWind.ActiveTime.Application.StatusManagement.ApplicationStatuses;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.JobEngine;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine;
using DustInTheWind.ActiveTime.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.Recording.StartRecording;

public sealed class StartRecordingUseCase : IRequestHandler<StartRecordingRequest>, IDisposable
{
    private readonly IUnitOfWork unitOfWork;
    private readonly Scribe scribe;
    private readonly EventBus eventBus;
    private readonly JobCollection jobCollection;
    private readonly StatusInfoService statusInfoService;

    public StartRecordingUseCase(IUnitOfWork unitOfWork, Scribe scribe, EventBus eventBus, JobCollection jobCollection,
        StatusInfoService statusInfoService)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.scribe = scribe ?? throw new ArgumentNullException(nameof(scribe));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.jobCollection = jobCollection ?? throw new ArgumentNullException(nameof(jobCollection));
        this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));
    }

    public async Task Handle(StartRecordingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            scribe.StampNew();
            jobCollection.Start(JobNames.Recorder);

            statusInfoService.SetStatus<RecorderStartedStatusMessage>();

            unitOfWork.Commit();
            unitOfWork.Dispose();

            RecorderStartedEvent recorderStartedEvent = new();
            await eventBus.Publish(recorderStartedEvent, cancellationToken);
        }
        finally
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        unitOfWork?.Dispose();
    }
}