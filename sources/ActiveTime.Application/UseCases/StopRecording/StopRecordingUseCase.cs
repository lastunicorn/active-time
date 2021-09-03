using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Infrastructure;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.StopRecording
{
    internal class StopRecordingUseCase : IRequestHandler<StopRecordingRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ScribeEx scribeEx;
        private readonly EventBus eventBus;
        private readonly ScheduledJobs scheduledJobs;

        public StopRecordingUseCase(IUnitOfWork unitOfWork, ScribeEx scribeEx, EventBus eventBus, ScheduledJobs scheduledJobs)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.scribeEx = scribeEx ?? throw new ArgumentNullException(nameof(scribeEx));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.scheduledJobs = scheduledJobs ?? throw new ArgumentNullException(nameof(scheduledJobs));
        }

        public Task<Unit> Handle(StopRecordingRequest request, CancellationToken cancellationToken)
        {
            if (request.DeleteLastRecord)
                scribeEx.DeleteCurrentTimeRecord();
            else
                scribeEx.Stamp();

            IJob recorderJob = scheduledJobs.Get("Recorder");
            recorderJob.Stop();

            unitOfWork.Commit();
            unitOfWork.Dispose();

            eventBus.Raise(EventNames.Recorder.Stopped);

            return Task.FromResult(Unit.Value);
        }
    }
}