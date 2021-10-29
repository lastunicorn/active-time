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
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.TimeRecords.PresentTimeRecords;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public class DayRecordsViewModel : ViewModelBase
    {
        private readonly IMediator mediator;

        private DayTimeInterval[] records;

        public DayTimeInterval[] Records
        {
            get => records;
            private set
            {
                records = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DayRecordsViewModel"/> class.
        /// </summary>
        public DayRecordsViewModel(IMediator mediator, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            _ = Initialize();

            eventBus.Subscribe(EventNames.CurrentDate.CurrentDateChanged, HandleCurrentDateChanged);
            eventBus.Subscribe(EventNames.Recorder.Started, HandleRecorderStarted);
            eventBus.Subscribe(EventNames.Recorder.Stopped, HandleRecorderStopped);
            eventBus.Subscribe(EventNames.Recorder.Stamped, HandleRecorderStamped);
        }

        private void HandleCurrentDateChanged(EventParameters parameters)
        {
            _ = Initialize();
        }

        private void HandleRecorderStarted(EventParameters parameters)
        {
            _ = Initialize();
        }

        private void HandleRecorderStopped(EventParameters parameters)
        {
            _ = Initialize();
        }

        private void HandleRecorderStamped(EventParameters parameters)
        {
            _ = Initialize();
        }

        private async Task Initialize()
        {
            PresentTimeRecordsRequest request = new PresentTimeRecordsRequest();
            PresentTimeRecordsResponse response = await mediator.Send(request);

            Records = response.Records;
        }
    }
}