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
using System.Windows;
using DustInTheWind.ActiveTime.Common.Services;
using DustInTheWind.ActiveTime.Common.UI.ShellNavigation;
using DustInTheWind.ActiveTime.Services;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace DustInTheWind.ActiveTime
{
    class Bootstrapper : UnityBootstrapper, IDisposable
    {
        protected override IModuleCatalog CreateModuleCatalog()
        {
            Uri uri = new Uri("/ActiveTime;component/ModuleCatalog.xaml", UriKind.Relative);
            ModuleCatalog moduleCatalog = Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(uri);

            return moduleCatalog;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            // Register WPF related services.
            Container.RegisterType<IApplicationService, ApplicationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IShellNavigator, ShellNavigator>(new ContainerControlledLifetimeManager());
            Container.RegisterType<DispatcherService, DispatcherService>(new ContainerControlledLifetimeManager());

            // Register business services.
            Container.RegisterType<ITimeProvider, CurrentTimeProvider>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IStatusInfoService, StatusInfoService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IStateService, StateService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IConfigurationService, ConfigurationService>();
        }

        protected override DependencyObject CreateShell()
        {
            return null;
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}
