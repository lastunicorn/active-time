using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Jobs;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Infrastructure;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.ToggleRecorder
{
    public class ToggleRecorderUseCase : IRequestHandler<ToggleRecorderRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ScribeEx scribeEx;
        private readonly EventBus eventBus;
        private readonly ScheduledJobs scheduledJobs;

        public ToggleRecorderUseCase(IUnitOfWork unitOfWork, ScribeEx scribeEx, EventBus eventBus, ScheduledJobs scheduledJobs)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.scribeEx = scribeEx ?? throw new ArgumentNullException(nameof(scribeEx));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.scheduledJobs = scheduledJobs ?? throw new ArgumentNullException(nameof(scheduledJobs));
        }

        public Task<Unit> Handle(ToggleRecorderRequest request, CancellationToken cancellationToken)
        {
            IJob recorderJob = scheduledJobs.Get("Recorder");

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

        private void Start(IJob recorderJob)
        {
            scribeEx.StampNew();
            recorderJob.Start();

            eventBus.Raise(EventNames.Recorder.Started);
        }

        private void Stop(IJob recorderJob)
        {
            scribeEx.Stamp();
            recorderJob.Stop();

            eventBus.Raise(EventNames.Recorder.Stopped);
        }
    }
}