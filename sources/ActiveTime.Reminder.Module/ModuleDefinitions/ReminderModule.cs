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
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Reminder.Module.Reminding;
using DustInTheWind.ActiveTime.Reminder.Module.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.Reminder.Module.ModuleDefinitions
{
    public class ReminderModule : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IConfigurationService configurationService;

        public ReminderModule(IUnityContainer unityContainer, IConfigurationService configurationService)
        {
            if (unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));
            if (configurationService == null) throw new ArgumentNullException(nameof(configurationService));

            this.unityContainer = unityContainer;
            this.configurationService = configurationService;
        }

        public void Initialize()
        {
            unityContainer.RegisterType<IReminder, Reminding.Reminder>();

            PauseReminder pauseReminder = CreatePauseReminder();
            unityContainer.RegisterInstance<IPauseReminder>(pauseReminder, new ContainerControlledLifetimeManager());
        }

        private PauseReminder CreatePauseReminder()
        {
            PauseReminder pauseReminder = unityContainer.Resolve<PauseReminder>();

            pauseReminder.PauseInterval = configurationService.ReminderPauseInterval;
            pauseReminder.SnoozeInterval = configurationService.ReminderSnoozeInterval;
            pauseReminder.StartMonitoring();

            return pauseReminder;
        }
    }
}
