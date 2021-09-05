using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Reporting;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.PresentOverview
{
    public class PresentOverviewUseCase : IRequestHandler<PresentOverviewRequest, PresentOverviewResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public PresentOverviewUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<PresentOverviewResponse> Handle(PresentOverviewRequest request, CancellationToken cancellationToken)
        {
            PresentOverviewResponse response = new PresentOverviewResponse
            {
                Report = new OverviewReport
                {
                    FirstDay = request.FirstDay,
                    LastDay = request.LastDay,
                    DayRecords = RetrieveDayRecords(request.FirstDay, request.LastDay)
                }
            };

            return Task.FromResult(response);
        }

        private List<DayRecord> RetrieveDayRecords(DateTime firstDay, DateTime lastDay)
        {
            ITimeRecordRepository timeRecordRepository = unitOfWork.TimeRecordRepository;

            List<DayRecord> dayRecords = unitOfWork.DayCommentRepository.GetByDate(firstDay, lastDay);

            foreach (DayRecord dayRecord in dayRecords)
            {
                dayRecord.TimeRecords = timeRecordRepository.GetByDate(dayRecord.Date).ToList();
            }

            return dayRecords;
        }
    }
}