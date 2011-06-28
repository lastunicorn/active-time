using DustInTheWind.ActiveTime.UI.Models;

namespace DustInTheWind.ActiveTime.UI.IViews
{
    public interface IStatisticsView : IViewBase
    {
        StatisticsModel Model { set; }
    }
}
