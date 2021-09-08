using System;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.UseCases.Stamp;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Jobs
{
    /// <summary>
    /// Periodically calls the scribe to update the time of the current record in the database.
    /// </summary>
    public class RecorderJob : JobBase
    {
        private readonly IMediator mediator;

        public override string Id { get; } = JobNames.Recorder;

        public RecorderJob(IMediator mediator, ITimer timer)
            : base(timer)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            RunInterval = TimeSpan.FromMinutes(1);
        }

        protected override async Task Execute()
        {
            StampRequest stampRequest = new StampRequest();
            await mediator.Send(stampRequest);
        }
    }
}