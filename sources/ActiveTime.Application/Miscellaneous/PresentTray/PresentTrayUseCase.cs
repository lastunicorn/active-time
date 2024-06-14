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
using DustInTheWind.ActiveTime.Domain;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Miscellaneous.PresentTray
{
    public class PresentTrayUseCase : IRequestHandler<PresentTrayRequest, PresentTrayResponse>
    {
        private readonly ScheduledJobs scheduledJobs;

        public PresentTrayUseCase(ScheduledJobs scheduledJobs)
        {
            this.scheduledJobs = scheduledJobs ?? throw new ArgumentNullException(nameof(scheduledJobs));
        }

        public Task<PresentTrayResponse> Handle(PresentTrayRequest request, CancellationToken cancellationToken)
        {
            IJob recordingJob = RetrieveRecordingJob();

            PresentTrayResponse response = new PresentTrayResponse
            {
                RecorderState = recordingJob.State
            };

            return Task.FromResult(response);
        }

        private IJob RetrieveRecordingJob()
        {
            IJob recordingJob = scheduledJobs.Get(JobNames.Recorder);
            return recordingJob;
        }
    }
}