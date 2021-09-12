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
using DustInTheWind.ActiveTime.Application.TimeReport.PresentTimeReport;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Logging;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using MediatR;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public class TimeReportViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private readonly ILogger logger;

        private TimeSpan activeTime;

        public TimeSpan ActiveTime
        {
            get => activeTime;
            private set
            {
                activeTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan totalTime;

        public TimeSpan TotalTime
        {
            get => totalTime;
            private set
            {
                totalTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan? beginTime;

        public TimeSpan? BeginTime
        {
            get => beginTime;
            set
            {
                beginTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan? estimatedEndTime;

        public TimeSpan? EstimatedEndTime
        {
            get => estimatedEndTime;
            set
            {
                estimatedEndTime = value;
                OnPropertyChanged();
            }
        }

        public TimeReportViewModel(IMediator mediator, EventBus eventBus, ILogger logger)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            eventBus.Subscribe(EventNames.CurrentDate.CurrentDateChanged, HandleCurrentDateChanged);
            eventBus.Subscribe(EventNames.Recorder.Stamped, HandleStamped);

            _ = Initialize();
        }

        private void HandleCurrentDateChanged(EventParameters parameters)
        {
            _ = Initialize();
        }

        private void HandleStamped(EventParameters parameters)
        {
            _ = Initialize();
        }

        private async Task Initialize()
        {
            try
            {
                PresentTimeReportRequest request = new PresentTimeReportRequest();
                PresentTimeReportResponse response = await mediator.Send(request);

                ActiveTime = response.ActiveTime;
                TotalTime = response.TotalTime;
                BeginTime = response.BeginTime;
                EstimatedEndTime = response.EstimatedEndTime;
            }
            catch (Exception ex)
            {
                logger.Log("ERROR: " + ex);
            }
        }
    }
}