// ActiveTime
// Copyright (C) 2011 Dust in the Wind
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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Events;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.ShellNavigation;
using Microsoft.Practices.Prism.Events;

namespace DustInTheWind.ActiveTime.TrayIconModule.ViewModels
{
    class TrayIconPresenter
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

        private readonly Icon iconOn;
        private readonly Icon iconOff;

        private readonly IRecorder recorder;
        private readonly IApplicationService applicationService;
        private readonly IShellNavigator shellNavigator;

        public TrayIconPresenter(IRecorder recorder, IEventAggregator eventAggregator, IApplicationService applicationService,
            IShellNavigator shellNavigator)
        {
            if (recorder == null)
                throw new ArgumentNullException("recorder");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            if (applicationService == null)
                throw new ArgumentNullException("applicationService");

            if (shellNavigator == null)
                throw new ArgumentNullException("shellNavigator");

            this.recorder = recorder;
            this.applicationService = applicationService;
            this.shellNavigator = shellNavigator;

            iconOn = Properties.Resources.tray_on;
            iconOff = Properties.Resources.tray_off;

            ApplicationExitEvent applicationExitEvent = eventAggregator.GetEvent<ApplicationExitEvent>();
            applicationExitEvent.Subscribe(OnApplicationExitEvent);

            recorder.Started += new EventHandler(recorder_Started);
            recorder.Stopped += new EventHandler(recorder_Stopped);
        }

        private void recorder_Stopped(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void recorder_Started(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void OnApplicationExitEvent(object o)
        {
            Hide();
        }

        private void RefreshView()
        {
            switch (recorder.State)
            {
                case RecorderState.Stopped:
                    SetIconOff();
                    SetStartMenuItemEnabled();
                    SetStopMenuItemDisabled();
                    break;

                case RecorderState.Running:
                    SetIconOn();
                    SetStartMenuItemDisabled();
                    SetStopMenuItemEnabled();
                    break;

                default:
                    break;
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

        private void SetStopMenuItemEnabled()
        {
            if (View != null)
                View.StopMenuItemEnabled = true;
        }

        private void SetStopMenuItemDisabled()
        {
            if (View != null)
                View.StopMenuItemEnabled = false;
        }

        private void SetStartMenuItemEnabled()
        {
            if (View != null)
                View.StartMenuItemEnabled = true;
        }

        private void SetStartMenuItemDisabled()
        {
            if (View != null)
                View.StartMenuItemEnabled = false;
        }

        public void StopAndDeleteClicked()
        {
            recorder.Stop(true);
        }

        public void StopClicked()
        {
            recorder.Stop();
        }

        public void StartClicked()
        {
            recorder.Start();
        }

        public void ExitClicked()
        {
            applicationService.Exit();
        }

        public void ShowClicked()
        {
            shellNavigator.Navigate(ShellNames.MainShell);
        }

        public void LeftDoubleClicked()
        {
            shellNavigator.Navigate(ShellNames.MainShell);
        }

        internal void AboutClicked()
        {
            shellNavigator.Navigate(ShellNames.AboutShell);
        }
    }
}
