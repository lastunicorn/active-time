using System.Windows.Input;

namespace DustInTheWind.ActiveTime.Presentation
{
    public interface ICommandFactory
    {
        T Create<T>()
            where T : ICommand;
    }
}