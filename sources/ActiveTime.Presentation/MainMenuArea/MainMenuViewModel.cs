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
using System.Windows.Input;
using DustInTheWind.ActiveTime.Domain.Presentation.ShellNavigation;
using DustInTheWind.ActiveTime.Domain.Services;
using DustInTheWind.ActiveTime.Infrastructure;
using DustInTheWind.ActiveTime.Infrastructure.EventModel;
using DustInTheWind.ActiveTime.Ports.LogAccess;
using DustInTheWind.ActiveTime.Presentation.Commands;

namespace DustInTheWind.ActiveTime.Presentation.MainMenuArea
{
    public class MainMenuViewModel : ViewModelBase
    {
        public ICommand ExitCommand { get; }

        public ICommand StartCommand { get; }

        public ICommand StopCommand { get; }

        public ICommand AboutCommand { get; }

        public MainMenuViewModel(IApplicationService applicationService, IShellNavigator shellNavigator,
            IRequestBus requestBus, ILogger logger, EventBus eventBus)
        {
            if (applicationService == null) throw new ArgumentNullException(nameof(applicationService));
            if (shellNavigator == null) throw new ArgumentNullException(nameof(shellNavigator));
            if (requestBus == null) throw new ArgumentNullException(nameof(requestBus));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));

            ExitCommand = new ExitCommand(applicationService);
            StartCommand = new StartRecorderCommand(requestBus, logger, eventBus);
            StopCommand = new StopRecorderCommand(requestBus, logger, eventBus);
            AboutCommand = new AboutCommand(shellNavigator);
        }
    }
}