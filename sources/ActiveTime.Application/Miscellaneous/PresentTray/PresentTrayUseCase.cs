using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.UseCases.PresentApplicationStatus;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.PresentTray
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
            IJob recordingJob = scheduledJobs.Get(JobNames.Recorder);

            PresentTrayResponse response = new PresentTrayResponse
            {
                RecorderState = recordingJob.State
            };

            return Task.FromResult(response);
        }
    }
}