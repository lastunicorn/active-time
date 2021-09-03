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
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using DustInTheWind.ActiveTime.Application.UseCases.PresentTray;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Infrastructure;
using DustInTheWind.ActiveTime.Common.Logging;
using DustInTheWind.ActiveTime.Common.Presentation;
using DustInTheWind.ActiveTime.Common.Presentation.ShellNavigation;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.TrayGui.Commands;
using DustInTheWind.ActiveTime.TrayGui.Views;
using MediatR;

namespace DustInTheWind.ActiveTime.TrayGui.ViewModels
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

        public ICommand ShowCommand { get; }

        public ICommand StartRecorderCommand { get; }

        public ICommand StopRecorderCommand { get; }

        public ICommand AboutCommand { get; }

        public ICommand ExitCommand { get; }

        private readonly Icon iconOn;
        private readonly Icon iconOff;

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

            iconOn = Properties.Resources.tray_on;
            iconOff = Properties.Resources.tray_off;

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

            if (response.IsRecorderRunning)
                SetIconOn();
            else
                SetIconOff();
        }

        private void SetIconOn()
        {
            if (View != null)
                View.Icon = iconOn;
        }

        private void SetIconOff()
        {
            if (View != null)
                View.Icon = iconOff;
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