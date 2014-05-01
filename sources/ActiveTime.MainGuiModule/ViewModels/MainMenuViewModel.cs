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
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.ShellNavigation;
using DustInTheWind.ActiveTime.Common.UI;
using Microsoft.Practices.Prism.Commands;

namespace DustInTheWind.ActiveTime.MainGuiModule.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        private readonly IApplicationService applicationService;
        private readonly IShellNavigator shellNavigator;
        private readonly IRecorderService recorder;

        public ICommand ExportCommand { get; private set; }

        public ICommand StatisticsCommand { get; private set; }

        public ICommand ExitCommand { get; private set; }

        public ICommand AboutCommand { get; private set; }

        public ICommand StartCommand { get; private set; }

        public ICommand StopCommand { get; private set; }

        public MainMenuViewModel(IApplicationService applicationService, IShellNavigator shellNavigator, IRecorderService recorder)
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

            ExportCommand = new DelegateCommand(OnExportCommandExecuted);
            StatisticsCommand = new DelegateCommand(OnStatisticsCommandExecuted);
            ExitCommand = new DelegateCommand(OnExitCommandExecuted);
            AboutCommand = new DelegateCommand(OnAboutCommandExecuted);
            StartCommand = new DelegateCommand(OnStartCommandExecuted);
            StopCommand = new DelegateCommand(OnStopCommandExecuted);
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
