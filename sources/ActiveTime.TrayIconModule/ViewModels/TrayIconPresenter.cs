// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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
using System.Windows.Input;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Common.UI.ShellNavigation;
using DustInTheWind.ActiveTime.Recording;
using DustInTheWind.ActiveTime.TrayIconModule.Commands;
using DustInTheWind.ActiveTime.TrayIconModule.Views;

namespace DustInTheWind.ActiveTime.TrayIconModule.ViewModels
{
    internal class TrayIconPresenter
    {
        private ITrayIconView view;
        public ITrayIconView View
        {
            get { return view; }
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

        private readonly IRecorderService recorder;
        private readonly IShellNavigator shellNavigator;

        public TrayIconPresenter(IRecorderService recorder, IApplicationService applicationService, IShellNavigator shellNavigator)
        {
            if (recorder == null) throw new ArgumentNullException(nameof(recorder));
            if (applicationService == null) throw new ArgumentNullException(nameof(applicationService));
            if (shellNavigator == null) throw new ArgumentNullException(nameof(shellNavigator));

            this.recorder = recorder;
            this.shellNavigator = shellNavigator;

            iconOn = Properties.Resources.tray_on;
            iconOff = Properties.Resources.tray_off;

            ShowCommand = new ShowCommand(shellNavigator);
            StartRecorderCommand = new StartRecorderCommand(recorder);
            StopRecorderCommand = new StopRecorderCommand(recorder);
            AboutCommand = new AboutCommand(shellNavigator);
            ExitCommand = new ExitCommand(applicationService);

            applicationService.Exiting += HandleApplicationServiceExiting;

            recorder.Started += HandleRecorderStarted;
            recorder.Stopped += HandleRecorderStopped;
        }

        private void HandleRecorderStarted(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void HandleRecorderStopped(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void HandleApplicationServiceExiting(object sender, EventArgs e)
        {
            Hide();
        }

        private void RefreshView()
        {
            switch (recorder.State)
            {
                case RecorderState.Stopped:
                    SetIconOff();
                    break;

                case RecorderState.Running:
                    SetIconOn();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Initialize()
        {
            RefreshView();
            Show();
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
