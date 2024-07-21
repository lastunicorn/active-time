using System;
using System.Windows;

namespace DustInTheWind.ActiveTime.Presentation.Services
{
    public interface IWindowFactory
    {
        Window Create(Type type);
    }
}