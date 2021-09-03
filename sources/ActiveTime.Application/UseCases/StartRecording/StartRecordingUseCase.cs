using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Infrastructure;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.StartRecording
{
    public class StartRecordingUseCase : IRequestHandler<StartRecordingRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ScribeEx scribeEx;
        private readonly EventBus eventBus;
        private readonly ScheduledJobs scheduledJobs;

        public StartRecordingUseCase(IUnitOfWork unitOfWork, ScribeEx scribeEx, EventBus eventBus, ScheduledJobs scheduledJobs)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.scribeEx = scribeEx ?? throw new ArgumentNullException(nameof(scribeEx));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.scheduledJobs = scheduledJobs ?? throw new ArgumentNullException(nameof(scheduledJobs));
        }

        public Task<Unit> Handle(StartRecordingRequest request, CancellationToken cancellationToken)
        {
            scribeEx.StampNew();

            IJob recorderJob = scheduledJobs.Get("Recorder");
            recorderJob.Start();

            unitOfWork.Commit();
            unitOfWork.Dispose();

            eventBus.Raise(EventNames.Recorder.Started);

            return Task.FromResult(Unit.Value);
        }
    }
}