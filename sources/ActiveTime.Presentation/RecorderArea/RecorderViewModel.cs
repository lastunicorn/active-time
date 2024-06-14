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
using DustInTheWind.ActiveTime.Application.UseCases.Recording.PresentRecorderState;
using DustInTheWind.ActiveTime.Application.UseCases.Recording.StartRecording;
using DustInTheWind.ActiveTime.Application.UseCases.Recording.StopRecording;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Presentation.Commands;

namespace DustInTheWind.ActiveTime.Presentation.RecorderArea;

public class RecorderViewModel : ViewModelBase
{
    private readonly IRequestBus requestBus;
    private bool isStarted;
    private bool isStopped;

    public bool IsStarted
    {
        get => isStarted;
        set
        {
            if (value == isStarted) return;
            isStarted = value;
            OnPropertyChanged();
        }
    }

    public bool IsStopped
    {
        get => isStopped;
        set
        {
            if (value == isStopped) return;
            isStopped = value;
            OnPropertyChanged();
        }
    }

    public StartRecorderCommand StartCommand { get; }

    public StopRecorderCommand StopCommand { get; }

    public RecorderViewModel(IRequestBus requestBus, EventBus eventBus, StartRecorderCommand startRecorderCommand, StopRecorderCommand stopRecorderCommand)
    {
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
        StartCommand = startRecorderCommand ?? throw new ArgumentNullException(nameof(startRecorderCommand));
        StopCommand = stopRecorderCommand ?? throw new ArgumentNullException(nameof(stopRecorderCommand));

        eventBus.Subscribe<RecorderStartedEvent>(HandleRecorderStartedAction);
        eventBus.Subscribe<RecorderStoppedEvent>(HandleRecorderStoppedAction);

        _ = Initialize();
    }

    private async Task Initialize()
    {
        PresentRecorderStateRequest request = new();
        PresentRecorderStateResponse response = await requestBus.Send<PresentRecorderStateRequest, PresentRecorderStateResponse>(request);

        IsStarted = response.IsRunning;
        IsStopped = !response.IsRunning;
    }

    private Task HandleRecorderStartedAction(RecorderStartedEvent ev, CancellationToken cancellationToken)
    {
        IsStarted = true;
        IsStopped = false;

        return Task.CompletedTask;
    }

    private Task HandleRecorderStoppedAction(RecorderStoppedEvent ev, CancellationToken cancellationToken)
    {
        IsStarted = false;
        IsStopped = true;

        return Task.CompletedTask;
    }
}