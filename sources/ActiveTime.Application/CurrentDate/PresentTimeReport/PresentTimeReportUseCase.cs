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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.Recording2;
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Ports.Persistence;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.CurrentDate.PresentTimeReport
{
    public sealed class PresentTimeReportUseCase : IRequestHandler<PresentTimeReportRequest, PresentTimeReportResponse>, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CurrentDay currentDay;

        public PresentTimeReportUseCase(IUnitOfWork unitOfWork, CurrentDay currentDay)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
        }

        public Task<PresentTimeReportResponse> Handle(PresentTimeReportRequest request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<TimeRecord> timeRecords = unitOfWork.TimeRecordRepository.GetByDate(currentDay.Date);
                PresentTimeReportResponse response = CreateResponse(timeRecords);

                return Task.FromResult(response);
            }
            finally
            {
                Dispose();
            }
        }

        private static PresentTimeReportResponse CreateResponse(IEnumerable<TimeRecord> timeRecords)
        {
            DayRecord dayRecord = new DayRecord(timeRecords);

            return new PresentTimeReportResponse
            {
                Records = dayRecord.AllIntervals.ToArray(),
                ActiveTime = dayRecord.TotalActiveTime,
                TotalTime = dayRecord.TotalTime,
                BeginTime = dayRecord.OverallBeginTime ?? TimeSpan.Zero,
                EstimatedEndTime = dayRecord.EstimatedEndTime ?? TimeSpan.Zero
            };
        }

        public void Dispose()
        {
            unitOfWork?.Dispose();
        }
    }
}