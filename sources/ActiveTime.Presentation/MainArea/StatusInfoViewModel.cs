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
using DustInTheWind.ActiveTime.Application.UseCases.Miscellaneous.PresentApplicationStatus;
using DustInTheWind.ActiveTime.Application.UseCases.Recording.StartRecording;
using DustInTheWind.ActiveTime.Application.UseCases.Recording.StopRecording;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Ports.LogAccess;
using DustInTheWind.ActiveTime.Presentation.Commands;

namespace DustInTheWind.ActiveTime.Presentation.MainArea;

/// <summary>
/// Contains the UI logic of the status bar displayed at the bottom of the main window.
/// </summary>
public class StatusInfoViewModel : ViewModelBase
{
    private readonly IRequestBus requestBus;
    private readonly ILogger logger;
    private string statusText;
    private bool isRecorderStarted;

    public string StatusText
    {
        get => statusText;
        private set
        {
            statusText = value;
            OnPropertyChanged();
        }
    }

    public bool IsRecorderStarted
    {
        get => isRecorderStarted;
        private set
        {
            isRecorderStarted = value;
            OnPropertyChanged();
        }
    }

    public ToggleRecorderCommand ToggleRecorderCommand { get; }

    public StatusInfoViewModel(IRequestBus requestBus, EventBus eventBus, ILogger logger)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        ToggleRecorderCommand = new ToggleRecorderCommand(requestBus);

        eventBus.Subscribe<RecorderStartedEvent>(HandleRecorderServiceStarted);
        eventBus.Subscribe<RecorderStoppedEvent>(HandleRecorderServiceStopped);
        eventBus.Subscribe<ApplicationStatusChangedEvent>(HandleApplicationStatusTextChanged);

        _ = Load();
    }

    private async Task Load()
    {
        try
        {
            PresentApplicationStatusRequest request = new();
            PresentApplicationStatusResponse response = await requestBus.Send<PresentApplicationStatusRequest, PresentApplicationStatusResponse>(request);

            IsRecorderStarted = response.IsRecorderStarted;
            StatusText = response.StatusText;
        }
        catch (Exception ex)
        {
            logger.Log("ERROR: " + ex);
        }
    }

    private Task HandleApplicationStatusTextChanged(ApplicationStatusChangedEvent ev, CancellationToken cancellationToken)
    {
        StatusText = ev.StatusText;

        return Task.CompletedTask;
    }

    private Task HandleRecorderServiceStarted(RecorderStartedEvent ev, CancellationToken cancellationToken)
    {
        IsRecorderStarted = true;

        return Task.CompletedTask;
    }

    private Task HandleRecorderServiceStopped(RecorderStoppedEvent ev, CancellationToken cancellationToken)
    {
        IsRecorderStarted = false;

        return Task.CompletedTask;
    }
}