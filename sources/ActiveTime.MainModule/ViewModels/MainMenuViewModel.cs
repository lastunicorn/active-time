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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Common;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace DustInTheWind.ActiveTime.MainModule.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        private readonly IApplicationService applicationService;
        private readonly IShellNavigator shellNavigator;

        private ICommand exportCommand;
        public ICommand ExportCommand
        {
            get { return exportCommand; }
        }

        private ICommand statisticsCommand;
        public ICommand StatisticsCommand
        {
            get { return statisticsCommand; }
        }

        private ICommand exitCommand;
        public ICommand ExitCommand
        {
            get { return exitCommand; }
        }

        private ICommand aboutCommand;
        public ICommand AboutCommand
        {
            get { return aboutCommand; }
        }

        public MainMenuViewModel(IApplicationService applicationService, IShellNavigator shellNavigator)
        {
            if (applicationService == null)
                throw new ArgumentNullException("applicationService");

            if (shellNavigator == null)
                throw new ArgumentNullException("shellNavigator");

            this.applicationService = applicationService;
            this.shellNavigator = shellNavigator;

            exportCommand = new DelegateCommand(OnExportCommandExecuted);
            statisticsCommand = new DelegateCommand(OnStatisticsCommandExecuted);
            exitCommand = new DelegateCommand(OnExitCommandExecuted);
            aboutCommand = new DelegateCommand(OnAboutCommandExecuted);
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
    }
}
