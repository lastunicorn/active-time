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

using DustInTheWind.ActiveTime.BusinessLogicModule.Services;
using DustInTheWind.ActiveTime.Common;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime.BusinessLogicModule.ModuleDefinitions
{
    /// <summary>
    /// This module initializes a few services needed by the application and also
    /// creates the main shell and some additional, less important shells like MessageShell
    /// and AboutShell. Additionally it registers the main views.
    /// </summary>
    public class BusinessLogicModule : IModule
    {
        private readonly IUnityContainer unityContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessLogicModule"/> class.
        /// </summary>
        /// <param name="unityContainer"></param>
        public BusinessLogicModule(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public void Initialize()
        {
            // Register passive services.
            unityContainer.RegisterType<ITimeProvider, CurrentTimeProvider>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IStatusInfoService, StatusInfoService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IStateService, StateService>(new ContainerControlledLifetimeManager());
        }
    }
}