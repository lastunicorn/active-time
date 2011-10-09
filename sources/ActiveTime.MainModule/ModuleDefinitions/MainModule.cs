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

using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.MainModule.Services;
using DustInTheWind.ActiveTime.MainModule.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.MainModule.ModuleDefinitions
{
    public class MainModule : IModule
    {
        private readonly IUnityContainer unityContainer;
        private readonly IRegionManager regionManager;
        private readonly IShellNavigator navigator;

        private IMainService mainService;

        public MainModule(IUnityContainer unityContainer, IRegionManager regionManager, IShellNavigator navigator)
        {
            this.unityContainer = unityContainer;
            this.regionManager = regionManager;
            this.navigator = navigator;
        }

        public void Initialize()
        {
            unityContainer.RegisterType<IApplicationService, ApplicationService>();
            unityContainer.RegisterType<ICommentsService, CommentsService>(new ContainerControlledLifetimeManager());

            SystemSessionService systemSessionService = unityContainer.Resolve<SystemSessionService>();
            unityContainer.RegisterInstance<SystemSessionService>(systemSessionService, new ContainerControlledLifetimeManager());

            regionManager.RegisterViewWithRegion(RegionNames.MainMenuRegion, typeof(MainMenuView));
            regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(MainView));

            unityContainer.RegisterType<object, MainView>(ViewNames.MainView);
            unityContainer.RegisterType<object, CommentsView>(ViewNames.CommentsView);

            navigator.RegisterShell(new ShellInfo(ShellNames.MainShell, typeof(MainWindow)));
            navigator.RegisterShell(new ShellInfo(ShellNames.MessageShell, typeof(PauseWindow)));
            navigator.RegisterShell(new ShellInfo(ShellNames.AboutShell, typeof(AboutWindow)));
        }
    }
}