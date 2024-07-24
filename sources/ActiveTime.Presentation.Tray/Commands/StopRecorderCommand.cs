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
using System.Windows.Input;
using DustInTheWind.ActiveTime.Application.UseCases.Recording.StartRecording;
using DustInTheWind.ActiveTime.Application.UseCases.Recording.StopRecording;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.JobEngine;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine;
using DustInTheWind.ActiveTime.Ports.LogAccess;

namespace DustInTheWind.ActiveTime.Presentation.Tray.Commands;

public class StopRecorderCommand : ICommand
{
    private readonly IRequestBus requestBus;
    private readonly ILog log;

    private JobState recorderState = JobState.Stopped;

    public JobState RecorderState
    {
        get => recorderState;
        set
        {
            recorderState = value;
            OnCanExecuteChanged();
        }
    }

    public event EventHandler CanExecuteChanged;

    public StopRecorderCommand(IRequestBus requestBus, ILog log, EventBus eventBus)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
        this.log = log ?? throw new ArgumentNullException(nameof(log));

        eventBus.Subscribe<RecorderStartedEvent>(HandleRecorderStarted);
        eventBus.Subscribe<RecorderStoppedEvent>(HandleRecorderStopped);
    }

    private Task HandleRecorderStarted(RecorderStartedEvent ev, CancellationToken cancellationToken)
    {
        RecorderState = JobState.Running;

        return Task.CompletedTask;
    }

    private Task HandleRecorderStopped(RecorderStoppedEvent ev, CancellationToken cancellationToken)
    {
        RecorderState = JobState.Stopped;

        return Task.CompletedTask;
    }

    public bool CanExecute(object parameter)
    {
        return recorderState == JobState.Running;
    }

    public void Execute(object parameter)
    {
        _ = StopRecording(parameter);
    }

    private async Task StopRecording(object parameter)
    {
        StopRecordingRequest request = new()
        {
            DeleteLastRecord = parameter is true
        };

        try
        {
            await requestBus.Send(request);
        }
        catch (Exception ex)
        {
            log.Write("ERROR: " + ex);
        }
    }

    protected virtual void OnCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}