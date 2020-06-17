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
using DustInTheWind.ActiveTime.Commands;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Recording;

namespace DustInTheWind.ActiveTime.ViewModels
{
    /// <summary>
    /// Contains the UI logic of the status bar displayed at the bottom of the main window.
    /// </summary>
    public class StatusInfoViewModel : ViewModelBase
    {
        private readonly IStatusInfoService statusInfoService;
        private string statusText;
        private bool isRecorderStarted;

        public string StatusText
        {
            get { return statusText; }
            private set
            {
                statusText = value;
                OnPropertyChanged();
            }
        }

        public bool IsRecorderStarted
        {
            get { return isRecorderStarted; }
            private set
            {
                isRecorderStarted = value;
                OnPropertyChanged();
            }
        }

        public ToggleRecorderCommand ToggleRecorderCommand { get; }

        public StatusInfoViewModel(IStatusInfoService statusInfoService, IRecorderService recorderService)
        {
            if (statusInfoService == null) throw new ArgumentNullException(nameof(statusInfoService));

            this.statusInfoService = statusInfoService;
            this.statusInfoService.StatusTextChanged += HandleStatusTextChanged;

            ToggleRecorderCommand = new ToggleRecorderCommand(recorderService);

            recorderService.Started += HandleRecorderServiceStarted;
            recorderService.Stopped += HandleRecorderServiceStopped;

            statusText = this.statusInfoService.StatusText;
            isRecorderStarted = recorderService.State == RecorderState.Running;
        }

        private void HandleStatusTextChanged(object s, EventArgs e)
        {
            StatusText = statusInfoService.StatusText;
        }

        private void HandleRecorderServiceStopped(object sender, EventArgs e)
        {
            IsRecorderStarted = false;
        }

        private void HandleRecorderServiceStarted(object sender, EventArgs e)
        {
            IsRecorderStarted = true;
        }
    }
}
