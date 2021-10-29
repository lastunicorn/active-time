using System;
using System.Windows;
using DustInTheWind.ActiveTime.Presentation.Services;

namespace DustInTheWind.ActiveTime
{
    internal class MicrosoftDiWindowFactory : IWindowFactory
    {
        private readonly IServiceProvider context;

        public MicrosoftDiWindowFactory(IServiceProvider context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Window Create(Type type)
        {
            return (Window)context.GetService(type);
        }
    }
}