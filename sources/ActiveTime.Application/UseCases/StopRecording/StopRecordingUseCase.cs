using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.StopRecording
{
    public class StopRecordingUseCase : IRequestHandler<StopRecordingRequest>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ScribeEx scribeEx;

        public StopRecordingUseCase(IUnitOfWork unitOfWork, ScribeEx scribeEx)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.scribeEx = scribeEx ?? throw new ArgumentNullException(nameof(scribeEx));
        }

        public Task<Unit> Handle(StopRecordingRequest request, CancellationToken cancellationToken)
        {
            if (request.DeleteLastRecord)
                scribeEx.DeleteCurrentTimeRecord();
            else
                scribeEx.Stamp();

            // todo: stop timer
            // todo: raise "recorder stopped" event

            unitOfWork.Commit();

            return Task.FromResult(Unit.Value);
        }
    }
}