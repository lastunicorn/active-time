// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application;
using DustInTheWind.ActiveTime.Application.UseCases.CurrentDate.DecrementDate;
using DustInTheWind.ActiveTime.Application.UseCases.CurrentDate.PresentTimeReport;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Ports.LogAccess;

namespace DustInTheWind.ActiveTime.Presentation.CalendarArea;

public class TimeReportViewModel : ViewModelBase
{
    private readonly IRequestBus requestBus;
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
        private set
        {
            beginTime = value;
            OnPropertyChanged();
        }
    }

    private TimeSpan? estimatedEndTime;

    public TimeSpan? EstimatedEndTime
    {
        get => estimatedEndTime;
        private set
        {
            estimatedEndTime = value;
            OnPropertyChanged();
        }
    }

    public TimeReportViewModel(IRequestBus requestBus, EventBus eventBus, ILogger logger)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        eventBus.Subscribe<CurrentDateChangedEvent>(HandleCurrentDateChanged);
        eventBus.Subscribe<RecorderStampedEvent>(HandleRecorderStamped);

        _ = Initialize();
    }

    private async Task HandleCurrentDateChanged(CurrentDateChangedEvent ev, CancellationToken cancellationToken)
    {
        await Initialize();
    }

    private async Task HandleRecorderStamped(RecorderStampedEvent ev, CancellationToken cancellationToken)
    {
        await Initialize();
    }

    private async Task Initialize()
    {
        try
        {
            PresentTimeReportRequest request = new();
            PresentTimeReportResponse response = await requestBus.Send<PresentTimeReportRequest, PresentTimeReportResponse>(request);

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