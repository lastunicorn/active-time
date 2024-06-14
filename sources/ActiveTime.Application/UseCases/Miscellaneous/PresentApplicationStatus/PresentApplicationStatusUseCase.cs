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

namespace DustInTheWind.ActiveTime.Application.UseCases.Miscellaneous.PresentApplicationStatus
{
    public class PresentApplicationStatusUseCase : IRequestHandler<PresentApplicationStatusRequest, PresentApplicationStatusResponse>
    {
        private readonly StatusInfoService statusInfoService;
        private readonly JobCollection jobCollection;

        public PresentApplicationStatusUseCase(StatusInfoService statusInfoService, JobCollection jobCollection)
        {
            this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));
            this.jobCollection = jobCollection ?? throw new ArgumentNullException(nameof(jobCollection));
        }

        public Task<PresentApplicationStatusResponse> Handle(PresentApplicationStatusRequest request, CancellationToken cancellationToken)
        {
            IJob recorderJob = RetrieveRecorderJob();

            PresentApplicationStatusResponse response = new PresentApplicationStatusResponse
            {
                IsRecorderStarted = recorderJob.State == JobState.Running,
                StatusText = statusInfoService.StatusText
            };

            return Task.FromResult(response);
        }

        private IJob RetrieveRecorderJob()
        {
            return jobCollection.Get(JobNames.Recorder);
        }
    }
}