using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common.Infrastructure;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.ToggleRecorder
{
    public class ToggleRecorderUseCase : IRequestHandler<ToggleRecorderRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ScribeEx scribeEx;
        private readonly EventBus eventBus;

        public ToggleRecorderUseCase(IUnitOfWork unitOfWork, ScribeEx scribeEx, EventBus eventBus)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.scribeEx = scribeEx ?? throw new ArgumentNullException(nameof(scribeEx));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public Task<Unit> Handle(ToggleRecorderRequest request, CancellationToken cancellationToken)
        {
            // todo: get the recorder state somehow.

            //switch (recorder.State)
            //{
            //    case RecorderState.Stopped:
            //        Start();
            //        break;

            //    case RecorderState.Running:
            //        Stop();
            //        break;
            //}

            return Task.FromResult(Unit.Value);
        }

        private void Start()
        {
            scribeEx.StampNew();

            // todo: start timer

            // todo: raise "recorder started" event
            eventBus.Raise("Recorder.Started");

            unitOfWork.Commit();

        }

        private void Stop()
        {
            scribeEx.Stamp();

            // todo: stop timer
            // todo: raise "recorder stopped" event
            eventBus.Raise("Recorder.Stopped");

            unitOfWork.Commit();
        }
    }
}