using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using DustInTheWind.ActiveTime.TrayIconModule.Views;

namespace DustInTheWind.ActiveTime.TrayIconModule.ModuleDefinitions
{
    public class TrayIconModule : IModule
    {
        private TrayIconView trayIconView;
        private IUnityContainer unityContainer;

        public TrayIconModule(IUnityContainer unityContainer)
        {
            if (unityContainer == null)
                throw new ArgumentNullException("unityContainer");

            this.unityContainer = unityContainer;
        }

        public void Initialize()
        {
            trayIconView = unityContainer.Resolve<TrayIconView>();
        }
    }
}
