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
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.Persistence;
using DustInTheWind.ActiveTime.Ports.SystemAccess;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Miscellaneous.PresentOverview
{
    public sealed class PresentOverviewUseCase : IRequestHandler<PresentOverviewRequest, PresentOverviewResponse>, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISystemClock systemClock;

        public PresentOverviewUseCase(IUnitOfWork unitOfWork, ISystemClock systemClock)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
        }

        public Task<PresentOverviewResponse> Handle(PresentOverviewRequest request, CancellationToken cancellationToken)
        {
            try
            {
                (DateTime firstDay, DateTime lastDay) = CalculateDateInterval(request);
                List<DateRecord> dayRecords = RetrieveDayRecords(firstDay, lastDay);

                PresentOverviewResponse response = new PresentOverviewResponse
                {
                    FirstDay = firstDay,
                    LastDay = lastDay,
                    DayRecords = dayRecords
                };

                return Task.FromResult(response);
            }
            finally
            {
                Dispose();
            }
        }

        private (DateTime, DateTime) CalculateDateInterval(PresentOverviewRequest request)
        {
            DateTime firstDay;
            DateTime lastDay;

            if (request.FirstDay == null || request.LastDay == null)
            {
                DateTime today = systemClock.GetCurrentDate();
                firstDay = today.AddDays(-29);
                lastDay = today;
            }
            else
            {
                firstDay = request.FirstDay.Value;
                lastDay = request.LastDay.Value;
            }

            return (firstDay, lastDay);
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

        public void Dispose()
        {
            unitOfWork?.Dispose();
        }
    }
}