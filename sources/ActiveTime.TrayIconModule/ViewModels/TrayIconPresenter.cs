// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
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
using System.Threading.Tasks;
using DustInTheWind.ActiveTime.Application.Miscellaneous.PresentTray;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Logging;
using DustInTheWind.ActiveTime.Common.Presentation;
using DustInTheWind.ActiveTime.Common.Presentation.ShellNavigation;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Infrastructure.JobModel;
using DustInTheWind.ActiveTime.Presentation.Tray.Commands;
using DustInTheWind.ActiveTime.Presentation.Tray.Views;
using MediatR;

namespace DustInTheWind.ActiveTime.Presentation.Tray.ViewModels
{
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
        private readonly IMediator mediator;

        public TrayIconPresenter(IApplicationService applicationService, IShellNavigator shellNavigator,
            IMediator mediator, ILogger logger, EventBus eventBus)
        {
            if (applicationService == null) throw new ArgumentNullException(nameof(applicationService));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));

            this.shellNavigator = shellNavigator ?? throw new ArgumentNullException(nameof(shellNavigator));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            ShowCommand = new ShowCommand(shellNavigator);
            StartRecorderCommand = new StartRecorderCommand(mediator, logger, eventBus);
            StopRecorderCommand = new StopRecorderCommand(mediator, logger, eventBus);
            AboutCommand = new AboutCommand(shellNavigator);
            ExitCommand = new ExitCommand(applicationService);

            applicationService.Exiting += HandleApplicationServiceExiting;

            eventBus.Subscribe(EventNames.Recorder.Started, HandleRecorderStarted);
            eventBus.Subscribe(EventNames.Recorder.Stopped, HandleRecorderStopped);
        }

        private void HandleRecorderStarted(EventParameters parameters)
        {
            SetIconOn();
        }

        private void HandleRecorderStopped(EventParameters parameters)
        {
            SetIconOff();
        }

        private void HandleApplicationServiceExiting(object sender, EventArgs e)
        {
            Hide();
        }

        private void Initialize()
        {
            _ = RefreshView();
            Show();
        }

        private async Task RefreshView()
        {
            PresentTrayRequest request = new PresentTrayRequest();
            PresentTrayResponse response = await mediator.Send(request);

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

        private void Show()
        {
            if (View != null)
                View.Visible = true;
        }

        private void Hide()
        {
            if (View != null)
                View.Visible = false;
        }

        public void LeftDoubleClicked()
        {
            shellNavigator.Navigate(ShellNames.MainShell);
        }
    }
}