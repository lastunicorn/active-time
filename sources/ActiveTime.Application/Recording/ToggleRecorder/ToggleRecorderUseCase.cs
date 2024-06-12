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
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Recording.ToggleRecorder
{
    public sealed class ToggleRecorderUseCase : IRequestHandler<ToggleRecorderRequest>, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly Scribe scribe;
        private readonly EventBus eventBus;
        private readonly ScheduledJobs scheduledJobs;

        public ToggleRecorderUseCase(IUnitOfWork unitOfWork, Scribe scribe, EventBus eventBus, ScheduledJobs scheduledJobs)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.scribe = scribe ?? throw new ArgumentNullException(nameof(scribe));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.scheduledJobs = scheduledJobs ?? throw new ArgumentNullException(nameof(scheduledJobs));
        }

        public Task Handle(ToggleRecorderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                IJob recorderJob = GetRecorderJob();

                switch (recorderJob.State)
                {
                    case JobState.Stopped:
                        Start(recorderJob);
                        break;

                    case JobState.Running:
                        Stop(recorderJob);
                        break;
                }

                unitOfWork.Commit();
                unitOfWork.Dispose();

                return Task.FromResult(Unit.Value);
            }
            finally
            {
                Dispose();
            }
        }

        private IJob GetRecorderJob()
        {
            IJob recorderJob = scheduledJobs.Get(JobNames.Recorder);
            return recorderJob;
        }

        private void Start(IJob recorderJob)
        {
            scribe.StampNew();
            recorderJob.Start();

            eventBus.Raise(EventNames.Recorder.Started);
        }

        private void Stop(IJob recorderJob)
        {
            recorderJob.Stop();
            scribe.Stamp();

            eventBus.Raise(EventNames.Recorder.Stopped);
        }

        public void Dispose()
        {
            unitOfWork?.Dispose();
        }
    }
}