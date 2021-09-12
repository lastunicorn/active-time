namespace DustInTheWind.ActiveTime.Presentation
{
    public interface IViewModelFactory
    {
        T Create<T>()
            where T : ViewModelBase;
    }
}