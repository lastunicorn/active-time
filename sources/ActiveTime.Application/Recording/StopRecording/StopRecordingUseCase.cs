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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.ApplicationStatuses;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Recording.StopRecording
{
    internal class StopRecordingUseCase : IRequestHandler<StopRecordingRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly Scribe scribe;
        private readonly EventBus eventBus;
        private readonly ScheduledJobs scheduledJobs;
        private readonly IStatusInfoService statusInfoService;

        public StopRecordingUseCase(IUnitOfWork unitOfWork, Scribe scribe, EventBus eventBus, ScheduledJobs scheduledJobs,
            IStatusInfoService statusInfoService)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.scribe = scribe ?? throw new ArgumentNullException(nameof(scribe));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.scheduledJobs = scheduledJobs ?? throw new ArgumentNullException(nameof(scheduledJobs));
            this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));
        }

        public Task Handle(StopRecordingRequest request, CancellationToken cancellationToken)
        {
            if (request.DeleteLastRecord)
                scribe.DeleteCurrentTimeRecord();
            else
                scribe.Stamp();

            scheduledJobs.Stop(JobNames.Recorder);
            eventBus.Raise(EventNames.Recorder.Stopped);
            statusInfoService.SetStatus(ApplicationStatus.Create<RecorderStoppedStatus>());

            unitOfWork.Commit();
            unitOfWork.Dispose();

            return Task.FromResult(Unit.Value);
        }
    }
}