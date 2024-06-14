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
using DustInTheWind.ActiveTime.Application.Recording.PresentRecorderState;
using DustInTheWind.ActiveTime.Application.Recording.StartRecording;
using DustInTheWind.ActiveTime.Application.Recording.StopRecording;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Presentation.Commands;

namespace DustInTheWind.ActiveTime.Presentation.RecorderArea;

public class RecorderViewModel : ViewModelBase
{
    private readonly IRequestBus requestBus;
    private string state;
    private const string RunningText = "Running";
    private const string StoppedText = "Stopped";

    public string State
    {
        get => state;
        private set
        {
            if (value == state) return;
            state = value;
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

        State = response.IsRunning
            ? RunningText
            : StoppedText;
    }

    private Task HandleRecorderStartedAction(RecorderStartedEvent ev, CancellationToken cancellationToken)
    {
        State = RunningText;

        return Task.CompletedTask;
    }

    private Task HandleRecorderStoppedAction(RecorderStoppedEvent ev, CancellationToken cancellationToken)
    {
        State = StoppedText;

        return Task.CompletedTask;
    }
}