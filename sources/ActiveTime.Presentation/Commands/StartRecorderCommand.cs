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
using DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine;
using DustInTheWind.ActiveTime.Ports.LogAccess;

namespace DustInTheWind.ActiveTime.Presentation.Commands;

public class StartRecorderCommand : CommandBase
{
    private readonly IRequestBus requestBus;
    private readonly ILog log;
    private bool canExecute;

    public StartRecorderCommand(IRequestBus requestBus, ILog log, EventBus eventBus)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
        this.log = log ?? throw new ArgumentNullException(nameof(log));

        eventBus.Subscribe<RecorderStartedEvent>(HandleRecorderStarted);
        eventBus.Subscribe<RecorderStoppedEvent>(HandleRecorderStopped);

        _ = Initialize();
    }

    private async Task Initialize()
    {
        PresentRecorderStateRequest request = new();
        PresentRecorderStateResponse response = await requestBus.Send<PresentRecorderStateRequest, PresentRecorderStateResponse>(request);

        canExecute = !response.IsRunning;
        OnCanExecuteChanged();
    }

    private Task HandleRecorderStarted(RecorderStartedEvent ev, CancellationToken cancellationToken)
    {
        canExecute = false;
        OnCanExecuteChanged();

        return Task.CompletedTask;
    }

    private Task HandleRecorderStopped(RecorderStoppedEvent ev, CancellationToken cancellationToken)
    {
        canExecute = true;
        OnCanExecuteChanged();

        return Task.CompletedTask;
    }

    public override bool CanExecute(object parameter)
    {
        return canExecute;
    }

    public override void Execute(object parameter)
    {
        _ = StartRecording();
    }

    private async Task StartRecording()
    {
        StartRecordingRequest request = new();

        try
        {
            await requestBus.Send(request);
        }
        catch (Exception ex)
        {
            log.Write("ERROR: " + ex);
        }
    }
}