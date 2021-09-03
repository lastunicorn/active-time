using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common.Infrastructure;
using DustInTheWind.ActiveTime.Common.Services;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.PresentApplicationStatus
{
    public class PresentApplicationStatusUseCase : IRequestHandler<PresentApplicationStatusResponse, PresentApplicationStatusResponse>
    {
        private readonly IStatusInfoService statusInfoService;
        private readonly ScheduledJobs scheduledJobs;

        public PresentApplicationStatusUseCase(IStatusInfoService statusInfoService, ScheduledJobs scheduledJobs)
        {
            this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));
            this.scheduledJobs = scheduledJobs ?? throw new ArgumentNullException(nameof(scheduledJobs));
        }

        public Task<PresentApplicationStatusResponse> Handle(PresentApplicationStatusResponse request, CancellationToken cancellationToken)
        {
            IJob recorderJob = scheduledJobs.Get("Recorder");

            PresentApplicationStatusResponse response = new PresentApplicationStatusResponse
            {
                IsRecorderStarted = recorderJob.State == JobState.Running,
                StatusText = statusInfoService.StatusText
            };

            return Task.FromResult(response);
        }
    }
}