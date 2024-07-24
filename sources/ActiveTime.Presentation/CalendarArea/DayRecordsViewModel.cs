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
using DustInTheWind.ActiveTime.Application.Recording2;
using DustInTheWind.ActiveTime.Application.UseCases.CurrentDate.DecrementDate;
using DustInTheWind.ActiveTime.Application.UseCases.CurrentDate.PresentTimeRecords;
using DustInTheWind.ActiveTime.Application.UseCases.Recording.StartRecording;
using DustInTheWind.ActiveTime.Application.UseCases.Recording.StopRecording;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine;

namespace DustInTheWind.ActiveTime.Presentation.CalendarArea;

public class DayRecordsViewModel : ViewModelBase
{
    private readonly IRequestBus requestBus;

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
    public DayRecordsViewModel(IRequestBus requestBus, EventBus eventBus)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        _ = Initialize();

        eventBus.Subscribe<CurrentDateChangedEvent>(HandleCurrentDateChanged);
        eventBus.Subscribe<RecorderStartedEvent>(HandleRecorderStarted);
        eventBus.Subscribe<RecorderStoppedEvent>(HandleRecorderStopped);
        eventBus.Subscribe<RecorderStampedEvent>(HandleRecorderStamped);
    }

    private async Task HandleCurrentDateChanged(CurrentDateChangedEvent ev, CancellationToken cancellationToken)
    {
        await Initialize();
    }

    private async Task HandleRecorderStarted(RecorderStartedEvent ev, CancellationToken cancellationToken)
    {
        await Initialize();
    }

    private async Task HandleRecorderStopped(RecorderStoppedEvent ev, CancellationToken cancellationToken)
    {
        await Initialize();
    }

    private async Task HandleRecorderStamped(RecorderStampedEvent ev, CancellationToken cancellationToken)
    {
        await Initialize();
    }

    private async Task Initialize()
    {
        PresentTimeRecordsRequest request = new();
        PresentTimeRecordsResponse response = await requestBus.Send<PresentTimeRecordsRequest, PresentTimeRecordsResponse>(request);

        Records = response.Records;
    }
}