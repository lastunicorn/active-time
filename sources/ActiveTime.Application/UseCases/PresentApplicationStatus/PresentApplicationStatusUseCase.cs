using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common.Services;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.UseCases.PresentApplicationStatus
{
    public class PresentApplicationStatusUseCase : IRequestHandler<PresentApplicationStatusResponse, PresentApplicationStatusResponse>
    {
        private readonly IStatusInfoService statusInfoService;

        public PresentApplicationStatusUseCase(IStatusInfoService statusInfoService)
        {
            this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));
        }

        public Task<PresentApplicationStatusResponse> Handle(PresentApplicationStatusResponse request, CancellationToken cancellationToken)
        {
            PresentApplicationStatusResponse response = new PresentApplicationStatusResponse
            {
                IsRecorderStarted = true,
                StatusText = statusInfoService.StatusText
            };

            return Task.FromResult(response);
        }
    }
}