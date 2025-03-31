using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Ports.SystemAccess;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.CurrentDate.ChangeDate
{
    internal class ChangeDateUseCase : IRequestHandler<ChangeDateRequest>
    {
        private readonly InMemoryState inMemoryState;
        private readonly ISystemClock systemClock;

        public ChangeDateUseCase(InMemoryState inMemoryState, ISystemClock systemClock)
        {
            this.inMemoryState = inMemoryState ?? throw new ArgumentNullException(nameof(inMemoryState));
            this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
        }

        public Task Handle(ChangeDateRequest request, CancellationToken cancellationToken)
        {
            inMemoryState.CurrentDate = request.Date ?? systemClock.GetCurrentDate();

            return Task.FromResult(Unit.Value);
        }
    }
}