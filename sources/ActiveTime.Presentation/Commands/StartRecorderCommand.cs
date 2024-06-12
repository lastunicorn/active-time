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
using DustInTheWind.ActiveTime.Application.Recording.StartRecording;
using DustInTheWind.ActiveTime.Application.Recording.StopRecording;
using DustInTheWind.ActiveTime.Common.Logging;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;

namespace DustInTheWind.ActiveTime.Presentation.Commands;

internal class StartRecorderCommand : CommandBase
{
    private readonly IRequestBus requestBus;
    private readonly ILogger logger;

    public StartRecorderCommand(IRequestBus requestBus, ILogger logger, EventBus eventBus)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        eventBus.Subscribe<RecorderStartedEvent>(HandleRecorderStarted);
        eventBus.Subscribe<RecorderStoppedEvent>(HandleRecorderStopped);
    }

    private Task HandleRecorderStarted(RecorderStartedEvent ev, CancellationToken cancellationToken)
    {
        OnCanExecuteChanged();

        return Task.CompletedTask;
    }

    private Task HandleRecorderStopped(RecorderStoppedEvent ev, CancellationToken cancellationToken)
    {
        OnCanExecuteChanged();

        return Task.CompletedTask;
    }

    public override bool CanExecute(object parameter)
    {
        // todo: check the recorder timer state.

        return true;
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
            logger.Log("ERROR: " + ex);
        }
    }
}