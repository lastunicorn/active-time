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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Miscellaneous.PresentApplicationStatus
{
    public class PresentApplicationStatusUseCase : IRequestHandler<PresentApplicationStatusRequest, PresentApplicationStatusResponse>
    {
        private readonly IStatusInfoService statusInfoService;
        private readonly ScheduledJobs scheduledJobs;

        public PresentApplicationStatusUseCase(IStatusInfoService statusInfoService, ScheduledJobs scheduledJobs)
        {
            this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));
            this.scheduledJobs = scheduledJobs ?? throw new ArgumentNullException(nameof(scheduledJobs));
        }

        public Task<PresentApplicationStatusResponse> Handle(PresentApplicationStatusRequest request, CancellationToken cancellationToken)
        {
            IJob recorderJob = scheduledJobs.Get(JobNames.Recorder);

            PresentApplicationStatusResponse response = new PresentApplicationStatusResponse
            {
                IsRecorderStarted = recorderJob.State == JobState.Running,
                StatusText = statusInfoService.StatusText
            };

            return Task.FromResult(response);
        }
    }
}