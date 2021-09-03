using System;
using System.Windows;
using Autofac;
using DustInTheWind.ActiveTime.Presentation.Services;

namespace DustInTheWind.ActiveTime
{
    internal class AutofacWindowFactory : IWindowFactory
    {
        private readonly IComponentContext context;

        public AutofacWindowFactory(IComponentContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Window Create(Type type)
        {
            return (Window)context.Resolve(type);
        }
    }
}