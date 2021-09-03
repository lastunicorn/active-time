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

        public StopRecordingUseCase(IUnitOfWork unitOfWork, ScribeEx scribeEx, EventBus eventBus)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.scribeEx = scribeEx ?? throw new ArgumentNullException(nameof(scribeEx));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public Task<Unit> Handle(StopRecordingRequest request, CancellationToken cancellationToken)
        {
            if (request.DeleteLastRecord)
                scribeEx.DeleteCurrentTimeRecord();
            else
                scribeEx.Stamp();

            // todo: stop timer

            eventBus.Raise(EventNames.Recorder.Stopped);

            unitOfWork.Commit();

            return Task.FromResult(Unit.Value);
        }
    }
}