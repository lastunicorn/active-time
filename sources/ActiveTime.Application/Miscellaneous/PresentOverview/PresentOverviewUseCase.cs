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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Reporting;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Miscellaneous.PresentOverview
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

        private List<DateRecord> RetrieveDayRecords(DateTime firstDay, DateTime lastDay)
        {
            ITimeRecordRepository timeRecordRepository = unitOfWork.TimeRecordRepository;

            List<DateRecord> dayRecords = unitOfWork.DateRecordRepository.GetByDate(firstDay, lastDay);

            foreach (DateRecord dayRecord in dayRecords)
            {
                dayRecord.TimeRecords = timeRecordRepository.GetByDate(dayRecord.Date).ToList();
            }

            return dayRecords;
        }
    }
}