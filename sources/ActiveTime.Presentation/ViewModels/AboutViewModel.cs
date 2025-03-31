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
using System.Timers;
using System.Windows.Input;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Presentation.Commands;
using Timer = System.Timers.Timer;

namespace DustInTheWind.ActiveTime.Presentation.ViewModels
{
    public class AboutViewModel : ViewModelBase, IDisposable
    {
        private readonly IApplicationService applicationService;
        private readonly Timer timer;

        private TimeSpan runTime;

        public string Version { get; }
        public DateTime? StartTime { get; }

        public TimeSpan RunTime
        {
            get => runTime;
            private set
            {
                runTime = value;
                OnPropertyChanged();
            }
        }

        public ICommand WindowClosed { get; }

        public AboutViewModel(IApplicationService applicationService)
        {
            this.applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));

            WindowClosed = new RelayCommand(HandleWindowClosed);

            Version version = applicationService.GetVersion();
            Version = version.ToString();

            StartTime = applicationService.StartTime;

            RunTime = applicationService.RunTime;

            timer = new Timer(200);
            timer.Elapsed += HandleTimerElapsed;
            timer.Start();
        }

        private void HandleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            RunTime = applicationService.RunTime;
        }

        private void HandleWindowClosed(object e)
        {
            if (timer == null)
                return;

            timer.Stop();
            timer.Dispose();
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}