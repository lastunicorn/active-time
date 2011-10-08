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
