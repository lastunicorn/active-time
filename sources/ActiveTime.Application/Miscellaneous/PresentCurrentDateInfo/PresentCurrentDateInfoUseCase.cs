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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Persistence;
using DustInTheWind.ActiveTime.Common.Recording;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Miscellaneous.PresentCurrentDateInfo
{
    public class PresentCurrentDateInfoUseCase : IRequestHandler<PresentCurrentDateInfoRequest, PresentCurrentDateInfoResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly InMemoryState inMemoryState;

        public PresentCurrentDateInfoUseCase(IUnitOfWork unitOfWork, InMemoryState inMemoryState)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.inMemoryState = inMemoryState ?? throw new ArgumentNullException(nameof(inMemoryState));
        }

        public Task<PresentCurrentDateInfoResponse> Handle(PresentCurrentDateInfoRequest request, CancellationToken cancellationToken)
        {
            if (inMemoryState.CurrentDate == null)
            {
                PresentCurrentDateInfoResponse response = CreateEmptyResponse();
                return Task.FromResult(response);
            }
            else
            {
                IEnumerable<TimeRecord> timeRecords = unitOfWork.TimeRecordRepository.GetByDate(inMemoryState.CurrentDate.Value);
                PresentCurrentDateInfoResponse response = CreateResponse(timeRecords);

                return Task.FromResult(response);
            }
        }

        private static PresentCurrentDateInfoResponse CreateResponse(IEnumerable<TimeRecord> timeRecords)
        {
            Common.Recording.DayRecord dayRecord = new Common.Recording.DayRecord(timeRecords);

            return new PresentCurrentDateInfoResponse
            {
                Records = dayRecord.AllIntervals.ToArray(),
                ActiveTime = dayRecord.TotalActiveTime,
                TotalTime = dayRecord.TotalTime,
                BeginTime = dayRecord.OverallBeginTime ?? TimeSpan.Zero,
                EstimatedEndTime = dayRecord.EstimatedEndTime ?? TimeSpan.Zero
            };
        }

        private static PresentCurrentDateInfoResponse CreateEmptyResponse()
        {
            return new PresentCurrentDateInfoResponse
            {
                Records = new DayTimeInterval[0],
                ActiveTime = TimeSpan.Zero,
                TotalTime = TimeSpan.Zero,
                BeginTime = TimeSpan.Zero,
                EstimatedEndTime = TimeSpan.Zero
            };
        }
    }
}