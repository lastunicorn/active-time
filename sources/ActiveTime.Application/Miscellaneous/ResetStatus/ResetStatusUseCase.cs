using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Domain.ApplicationStatuses;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.Miscellaneous.ResetStatus
{
    internal class ResetStatusUseCase : IRequestHandler<ResetStatusRequest>
    {
        private readonly StatusInfoService statusInfoService;

        public ResetStatusUseCase(StatusInfoService statusInfoService)
        {
            this.statusInfoService = statusInfoService ?? throw new ArgumentNullException(nameof(statusInfoService));
        }

        public Task Handle(ResetStatusRequest request, CancellationToken cancellationToken)
        {
            statusInfoService.SetStatus(ApplicationStatus.Create<ReadyStatus>());

            return Task.FromResult(Unit.Value);
        }
    }
}