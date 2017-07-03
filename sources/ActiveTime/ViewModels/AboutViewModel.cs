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
using System.Timers;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;

namespace DustInTheWind.ActiveTime.ViewModels
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
            get { return runTime; }
            set
            {
                runTime = value;
                OnPropertyChanged();
            }
        }

        public AboutViewModel(IApplicationService applicationService)
        {
            this.applicationService = applicationService;

            if (applicationService == null) throw new ArgumentNullException(nameof(applicationService));

            Version version = applicationService.GetVersion();
            Version = version.ToString();

            StartTime = applicationService.StartTime;

            RunTime = applicationService.RunTime;

            timer = new Timer(1000 * 60);
            timer.Elapsed += HandleTimerElapsed;
        }

        private void HandleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            RunTime = applicationService.RunTime;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
