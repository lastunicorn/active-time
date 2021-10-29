using System;
using Autofac;
using DustInTheWind.ActiveTime.Presentation;

namespace DustInTheWind.ActiveTime
{
    internal class ViewModelFactory : IViewModelFactory
    {
        private readonly IComponentContext context;

        public ViewModelFactory(IComponentContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public T Create<T>()
            where T : ViewModelBase
        {
            return context.Resolve<T>();
        }
    }
}