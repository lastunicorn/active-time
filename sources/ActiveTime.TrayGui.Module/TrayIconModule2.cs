using System;
using DustInTheWind.ActiveTime.TrayGui.Views;

namespace DustInTheWind.ActiveTime.TrayGui.Module
{
    public class TrayIconModule2
    {
        private TrayIconView trayIconView;

        private readonly IServiceProvider container;

        public TrayIconModule2(IServiceProvider container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void Initialize()
        {
            trayIconView = container.GetService(typeof(TrayIconView)) as TrayIconView;
        }
    }
}