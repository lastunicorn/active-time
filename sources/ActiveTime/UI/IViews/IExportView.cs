using DustInTheWind.ActiveTime.UI.Models;

namespace DustInTheWind.ActiveTime.UI.IViews
{
    public interface IExportView : IViewBase
    {
        ExportModel Model { set; }
        string RequestNewExportFileName();
    }
}
