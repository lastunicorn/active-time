using System;
using System.Windows.Input;
using Autofac;
using DustInTheWind.ActiveTime.Presentation;

namespace DustInTheWind.ActiveTime
{
    internal class CommandFactory : ICommandFactory
    {
        private readonly IComponentContext context;

        public CommandFactory(IComponentContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public T Create<T>()
            where T : ICommand
        {
            return context.Resolve<T>();
        }
    }
}