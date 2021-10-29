using System;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.System;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Application.CurrentDate.ChangeDate
{
    internal class ChangeDateUseCase : IRequestHandler<ChangeDateRequest>
    {
        private readonly CurrentDay currentDay;
        private readonly ISystemClock systemClock;
        private readonly EventBus eventBus;

        public ChangeDateUseCase(CurrentDay currentDay, ISystemClock systemClock, EventBus eventBus)
        {
            this.currentDay = currentDay ?? throw new ArgumentNullException(nameof(currentDay));
            this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public Task<Unit> Handle(ChangeDateRequest request, CancellationToken cancellationToken)
        {
            currentDay.Date = request.Date ?? systemClock.GetCurrentDate();
            RaiseCurrentDateChangedEvent();
            
            return Task.FromResult(Unit.Value);
        }

        private void RaiseCurrentDateChangedEvent()
        {
            EventParameters eventParameters = new EventParameters();
            eventParameters.Add("Date", currentDay.Date);

            eventBus.Raise(EventNames.CurrentDate.CurrentDateChanged, eventParameters);
        }
    }
}