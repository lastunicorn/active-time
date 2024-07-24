using System.Windows;

namespace DustInTheWind.ActiveTime.Infrastructure.Wpf.ShellEngine
{
    public interface IWindowFactory
    {
        Window Create(Type type);
    }
}