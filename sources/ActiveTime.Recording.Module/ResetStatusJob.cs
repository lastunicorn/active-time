using System;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.Miscellaneous.ResetStatus;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Jobs
{
    public class ResetStatusJob : OneTimeJob
    {
        private readonly IMediator mediator;

        public override string Id { get; } = JobNames.ResetStatus;

        public ResetStatusJob(IMediator mediator, ITimer timer)
            : base(timer)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task DoExecute()
        {
            ResetStatusRequest stampRequest = new ResetStatusRequest();
            await mediator.Send(stampRequest);
        }
    }
}