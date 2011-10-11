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
using DustInTheWind.ActiveTime.Common.ShellNavigation;
using DustInTheWind.ActiveTime.ShellNavigationModule.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.ShellNavigationModule.ModuleDefinitions
{
    public class ShellNavigationModule : IModule
    {
        private readonly IUnityContainer unityContainer;

        public ShellNavigationModule(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public void Initialize()
        {
            unityContainer.RegisterType<IShellNavigator, ShellNavigator>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<DispatcherService, DispatcherService>(new ContainerControlledLifetimeManager());
        }
    }
}