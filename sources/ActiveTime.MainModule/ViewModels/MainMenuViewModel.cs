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
using System.Windows.Input;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.ShellNavigation;
using Microsoft.Practices.Prism.Commands;
using DustInTheWind.ActiveTime.Common.Recording;

namespace DustInTheWind.ActiveTime.MainModule.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        private readonly IApplicationService applicationService;
        private readonly IShellNavigator shellNavigator;
        private readonly IRecorder recorder;

        private readonly ICommand exportCommand;
        public ICommand ExportCommand
        {
            get { return exportCommand; }
        }

        private readonly ICommand statisticsCommand;
        public ICommand StatisticsCommand
        {
            get { return statisticsCommand; }
        }

        private readonly ICommand exitCommand;
        public ICommand ExitCommand
        {
            get { return exitCommand; }
        }

        private readonly ICommand aboutCommand;
        public ICommand AboutCommand
        {
            get { return aboutCommand; }
        }

        private readonly ICommand startCommand;
        public ICommand StartCommand
        {
            get { return startCommand; }
        }

        private readonly ICommand stopCommand;
        public ICommand StopCommand
        {
            get { return stopCommand; }
        }

        public MainMenuViewModel(IApplicationService applicationService, IShellNavigator shellNavigator, IRecorder recorder)
        {
            if (applicationService == null)
                throw new ArgumentNullException("applicationService");

            if (shellNavigator == null)
                throw new ArgumentNullException("shellNavigator");

            if (recorder == null)
                throw new ArgumentNullException("recorder");

            this.applicationService = applicationService;
            this.shellNavigator = shellNavigator;
            this.recorder = recorder;

            exportCommand = new DelegateCommand(OnExportCommandExecuted);
            statisticsCommand = new DelegateCommand(OnStatisticsCommandExecuted);
            exitCommand = new DelegateCommand(OnExitCommandExecuted);
            aboutCommand = new DelegateCommand(OnAboutCommandExecuted);
            startCommand = new DelegateCommand(OnStartCommandExecuted);
            stopCommand = new DelegateCommand(OnStopCommandExecuted);
        }

        private void OnExportCommandExecuted()
        {
        }

        private void OnStatisticsCommandExecuted()
        {
        }

        private void OnExitCommandExecuted()
        {
            applicationService.Exit();
        }

        private void OnAboutCommandExecuted()
        {
            shellNavigator.Navigate(ShellNames.AboutShell);
        }

        private void OnStartCommandExecuted()
        {
            recorder.Start();
        }

        private void OnStopCommandExecuted()
        {
            recorder.Stop();
        }
    }
}
