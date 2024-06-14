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
using DustInTheWind.ActiveTime.Application.Miscellaneous.PresentTray;
using DustInTheWind.ActiveTime.Application.Recording.StartRecording;
using DustInTheWind.ActiveTime.Application.Recording.StopRecording;
using DustInTheWind.ActiveTime.Domain.Presentation.ShellNavigation;
using DustInTheWind.ActiveTime.Domain.Services;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using DustInTheWind.ActiveTime.Ports.LogAccess;
using DustInTheWind.ActiveTime.Presentation.Tray.Commands;
using DustInTheWind.ActiveTime.Presentation.Tray.Views;

namespace DustInTheWind.ActiveTime.Presentation.Tray.ViewModels;

public class TrayIconPresenter
{
    private ITrayIconView view;

    public ITrayIconView View
    {
        get => view;
        set
        {
            view = value;
            if (view != null) Initialize();
        }
    }

    public ShowCommand ShowCommand { get; }

    public StartRecorderCommand StartRecorderCommand { get; }

    public StopRecorderCommand StopRecorderCommand { get; }

    public AboutCommand AboutCommand { get; }

    public ExitCommand ExitCommand { get; }

    private readonly IShellNavigator shellNavigator;
    private readonly IRequestBus requestBus;

    public TrayIconPresenter(IApplicationService applicationService, IShellNavigator shellNavigator,
        IRequestBus requestBus, ILogger logger, EventBus eventBus)
    {
        if (applicationService == null) throw new ArgumentNullException(nameof(applicationService));
        if (logger == null) throw new ArgumentNullException(nameof(logger));
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));

        this.shellNavigator = shellNavigator ?? throw new ArgumentNullException(nameof(shellNavigator));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        ShowCommand = new ShowCommand(shellNavigator);
        StartRecorderCommand = new StartRecorderCommand(requestBus, logger, eventBus);
        StopRecorderCommand = new StopRecorderCommand(requestBus, logger, eventBus);
        AboutCommand = new AboutCommand(shellNavigator);
        ExitCommand = new ExitCommand(applicationService);

        applicationService.Exiting += HandleApplicationServiceExiting;

        eventBus.Subscribe<RecorderStartedEvent>(HandleRecorderStarted);
        eventBus.Subscribe<RecorderStoppedEvent>(HandleRecorderStopped);
    }

    private void HandleApplicationServiceExiting(object sender, EventArgs e)
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
        Show();
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
            View.Visible = true;
    }

    public void Hide()
    {
        if (View != null)
            View.Visible = false;
    }
}