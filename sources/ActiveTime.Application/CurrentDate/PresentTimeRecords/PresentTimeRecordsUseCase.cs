using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.TimeRecords.PresentTimeRecords
{
    internal class PresentTimeRecordsUseCase : IRequestHandler<PresentTimeRecordsRequest, PresentTimeRecordsResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly InMemoryState inMemoryState;

        public PresentTimeRecordsUseCase(IUnitOfWork unitOfWork, InMemoryState inMemoryState)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.inMemoryState = inMemoryState ?? throw new ArgumentNullException(nameof(inMemoryState));
        }

        public Task<PresentTimeRecordsResponse> Handle(PresentTimeRecordsRequest request, CancellationToken cancellationToken)
        {
            DateTime currentDate = inMemoryState.CurrentDate;
            IEnumerable<TimeRecord> timeRecords = unitOfWork.TimeRecordRepository.GetByDate(currentDate);

            DayRecord dayRecord = new DayRecord(timeRecords);

            PresentTimeRecordsResponse response = new PresentTimeRecordsResponse
            {
                Records = dayRecord.AllIntervals.ToArray()
            };

            return Task.FromResult(response);
        }
    }
}