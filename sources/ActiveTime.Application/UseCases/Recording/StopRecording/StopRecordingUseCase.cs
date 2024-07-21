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
using DustInTheWind.ActiveTime.Application.Recording2;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Domain.ApplicationStatuses;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseModel;
using DustInTheWind.ActiveTime.Ports.Persistence;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.Recording.StopRecording;

internal sealed class StopRecordingUseCase : IRequestHandler<StopRecordingRequest>, IDisposable
{
    private readonly IUnitOfWork unitOfWork;
    private readonly Scribe scribe;
    private readonly EventBus eventBus;
    private readonly JobCollection jobCollection;
    private readonly StatusInfoService statusInfoService;

    public StopRecordingUseCase(IUnitOfWork unitOfWork, Scribe scribe, EventBus eventBus, JobCollection jobCollection,
        StatusInfoService statusInfoService)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.scribe = scribe ?? throw new ArgumentNullException(nameof(scribe));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.jobCollection = jobCollection ?? throw new ArgumentNullException(nameof(jobCollection));
        this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));
    }

    public async Task Handle(StopRecordingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.DeleteLastRecord)
                scribe.DeleteCurrentTimeRecord();
            else
                scribe.Stamp();

            jobCollection.Stop(JobNames.Recorder);

            statusInfoService.SetStatus<RecorderStoppedStatusMessage>();

            unitOfWork.Commit();
            unitOfWork.Dispose();

            RecorderStoppedEvent recorderStoppedEvent = new();
            await eventBus.Publish(recorderStoppedEvent, cancellationToken);
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