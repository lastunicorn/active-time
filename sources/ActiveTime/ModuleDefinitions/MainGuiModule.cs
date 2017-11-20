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

using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Common.UI.ShellNavigation;
using DustInTheWind.ActiveTime.Logging;
using DustInTheWind.ActiveTime.Services;
using DustInTheWind.ActiveTime.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.ModuleDefinitions
{
    public class MainGuiModule : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IRegionManager regionManager;
        private readonly IShellNavigator shellNavigator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainGuiModule"/> class.
        /// </summary>
        public MainGuiModule(IUnityContainer unityContainer, IRegionManager regionManager, IShellNavigator shellNavigator)
        {
            this.unityContainer = unityContainer;
            this.regionManager = regionManager;
            this.shellNavigator = shellNavigator;
        }

        public void Initialize()
        {
            // Register services.
            unityContainer.RegisterType<ILogger, Logger>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ICurrentDay, CurrentDay>(new ContainerControlledLifetimeManager());

            // Register views in regions.
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(FrontView));

            // register views in container. (Needed for view navigation.)
            unityContainer.RegisterType<object, CommentsView>(ViewNames.CommentsView);
            unityContainer.RegisterType<object, DayRecordsView>(ViewNames.DayRecordsView);

            // Register shells in the shell navigator. (Needed for shell navigation.)
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.MainShell, typeof(MainWindow)));
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.MessageShell, typeof(MessageWindow), ShellNames.MainShell));
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.AboutShell, typeof(AboutWindow), ShellNames.MainShell));
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.OverviewShell, typeof(OverviewWindow), ShellNames.MainShell));
            shellNavigator.RegisterShell(new ShellInfo(ShellNames.CalendarShell, typeof(CalendarWindow), ShellNames.MainShell));
        }
    }
}
