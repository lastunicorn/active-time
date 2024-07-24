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

using System.Timers;
using System.Windows.Input;
using DustInTheWind.ActiveTime.Infrastructure.Wpf;
using DustInTheWind.ActiveTime.Presentation.Commands;
using Timer = System.Timers.Timer;

namespace DustInTheWind.ActiveTime.Presentation.AboutArea
{
    public class AboutViewModel : ViewModelBase, IDisposable
    {
        private readonly IApplication application;
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

        public AboutViewModel(IApplication application)
        {
            this.application = application ?? throw new ArgumentNullException(nameof(application));

            WindowClosed = new RelayCommand(HandleWindowClosed);

            Version version = application.GetVersion();
            Version = version.ToString();

            StartTime = application.StartTime;

            RunTime = application.RunTime;

            timer = new Timer(200);
            timer.Elapsed += HandleTimerElapsed;
            timer.Start();
        }

        private void HandleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            RunTime = application.RunTime;
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