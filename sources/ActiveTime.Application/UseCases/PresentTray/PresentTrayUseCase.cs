using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common.Infrastructure;
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
            IJob recordingJob = scheduledJobs.Get("Recorder");

            PresentTrayResponse response = new PresentTrayResponse
            {
                IsRecorderRunning = recordingJob.State == JobState.Running
            };

            return Task.FromResult(response);
        }
    }
}