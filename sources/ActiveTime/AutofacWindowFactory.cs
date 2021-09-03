using System;
using System.Windows;
using Autofac;
using DustInTheWind.ActiveTime.Presentation.Services;

namespace DustInTheWind.ActiveTime
{
    internal class AutofacWindowFactory : IWindowFactory
    {
        private readonly ILifetimeScope container;

        public AutofacWindowFactory(ILifetimeScope container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public Window Create(Type type)
        {
            return (Window)container.Resolve(type);
        }
    }
}