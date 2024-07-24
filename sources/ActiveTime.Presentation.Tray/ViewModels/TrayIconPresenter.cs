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
using DustInTheWind.ActiveTime.Application.UseCases.Miscellaneous.PresentTray;
using DustInTheWind.ActiveTime.Application.UseCases.Recording.StartRecording;
using DustInTheWind.ActiveTime.Application.UseCases.Recording.StopRecording;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.JobEngine;
using DustInTheWind.ActiveTime.Infrastructure.UseCaseEngine;
using DustInTheWind.ActiveTime.Infrastructure.Wpf;
using DustInTheWind.ActiveTime.Infrastructure.Wpf.ShellEngine;
using DustInTheWind.ActiveTime.Ports.LogAccess;
using DustInTheWind.ActiveTime.Presentation.Tray.Commands;
using DustInTheWind.ActiveTime.Presentation.Tray.Views;

namespace DustInTheWind.ActiveTime.Presentation.Tray.ViewModels;

public class TrayIconPresenter
{
    private readonly IApplication application;
    private readonly IRequestBus requestBus;
    private readonly EventBus eventBus;

    public ITrayIconView View { get; set; }

    public ShowCommand ShowCommand { get; }

    public StartRecorderCommand StartRecorderCommand { get; }

    public StopRecorderCommand StopRecorderCommand { get; }

    public AboutCommand AboutCommand { get; }

    public ExitCommand ExitCommand { get; }

    public TrayIconPresenter(IApplication application, IShellNavigator shellNavigator,
        IRequestBus requestBus, ILog log, EventBus eventBus)
    {
        if (shellNavigator == null) throw new ArgumentNullException(nameof(shellNavigator));
        if (log == null) throw new ArgumentNullException(nameof(log));

        this.application = application ?? throw new ArgumentNullException(nameof(application));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        ShowCommand = new ShowCommand(shellNavigator);
        StartRecorderCommand = new StartRecorderCommand(requestBus, log, eventBus);
        StopRecorderCommand = new StopRecorderCommand(requestBus, log, eventBus);
        AboutCommand = new AboutCommand(shellNavigator);
        ExitCommand = new ExitCommand(application);
    }

    private void HandleApplicationExiting(object sender, EventArgs e)
    {
        Hide();
    }

    private Task HandleRecorderStarted(RecorderStartedEvent ev, CancellationToken cancellationToken)
    {
        SetIconOn();

        return Task.CompletedTask;
    }

    private Task HandleRecorderStopped(RecorderStoppedEvent ev, CancellationToken cancellationToken)
    {
        SetIconOff();

        return Task.CompletedTask;
    }

    private void Initialize()
    {
        _ = RefreshView();
    }

    private async Task RefreshView()
    {
        PresentTrayRequest request = new();
        PresentTrayResponse response = await requestBus.Send<PresentTrayRequest, PresentTrayResponse>(request);

        if (response.RecorderState == JobState.Running)
            SetIconOn();
        else
            SetIconOff();

        StartRecorderCommand.RecorderState = response.RecorderState;
        StopRecorderCommand.RecorderState = response.RecorderState;
    }

    private void SetIconOn()
    {
        if (View != null)
            View.IconState = TrayIconState.On;
    }

    private void SetIconOff()
    {
        if (View != null)
            View.IconState = TrayIconState.Off;
    }

    public void Show()
    {
        if (View != null)
        {
            application.Exiting += HandleApplicationExiting;

            eventBus.Subscribe<RecorderStartedEvent>(HandleRecorderStarted);
            eventBus.Subscribe<RecorderStoppedEvent>(HandleRecorderStopped);

            Initialize();

            View.Visible = true;
        }
    }

    private void Hide()
    {
        if (View != null)
        {
            application.Exiting -= HandleApplicationExiting;

            eventBus.Unsubscribe<RecorderStartedEvent>(HandleRecorderStarted);
            eventBus.Unsubscribe<RecorderStoppedEvent>(HandleRecorderStopped);

            View.Visible = false;
        }
    }
}