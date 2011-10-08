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
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Events;
using System.Windows;
using DustInTheWind.ActiveTime.MainModule.Views;

namespace DustInTheWind.ActiveTime.MainModule.Services
{
    class ApplicationService : IApplicationService
    {
        private IEventAggregator eventAggregator;
        private IUnityContainer unityContainer;

        public ApplicationService(IUnityContainer unityContainer, IEventAggregator eventAggregator)
        {
            if (unityContainer == null)
                throw new ArgumentNullException("unityContainer");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            this.unityContainer = unityContainer;
            this.eventAggregator = eventAggregator;
        }

        public void Exit()
        {
            ApplicationExitEvent applicationExitEvent = eventAggregator.GetEvent<ApplicationExitEvent>();
            if (applicationExitEvent != null)
                applicationExitEvent.Publish(null);

            Application.Current.Shutdown();
        }

        //public void Dispatch(Delegate method)
        //{
        //    App.Current.Dispatcher.Invoke(method);
        //}


        public void ShowMainWindow()
        {
            Application.Current.Dispatcher.Invoke(new Action(ShowMainWindowInternal));
        }

        private Shell shell;

        private void ShowMainWindowInternal()
        {
            if (shell == null)
                CreateShell();

            shell.Show();
            shell.Activate();
        }

        private void CreateShell()
        {
            shell = new Shell();
            shell.Closed += (s, e) =>
            {
                RegionManager.SetRegionManager(shell, null);
                RegionManager.UpdateRegions();
                shell = null;
            };

            RegionManager.SetRegionManager(shell, unityContainer.Resolve<IRegionManager>());
            RegionManager.UpdateRegions();
        }
    }
}
