using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
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
            CreateNewRecord();
            StartRecorder();

            unitOfWork.Commit();
            unitOfWork.Dispose();

            return Task.FromResult(Unit.Value);
        }

        private void CreateNewRecord()
        {
            scribeEx.StampNew();
        }

        private void StartRecorder()
        {
            IJob recorderJob = scheduledJobs.Get(JobNames.Recorder);
            recorderJob.Start();

            eventBus.Raise(EventNames.Recorder.Started);
        }
    }
}