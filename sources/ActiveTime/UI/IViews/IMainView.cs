using System.Collections;

namespace DustInTheWind.ActiveTime.UI.IViews
{
    internal interface IMainView : IViewBase
    {
        void Hide();

        IList SelectedRecords { get; }

        bool Confirm(string text, string caption);

        void ExitApplication();

        void ShowExportWindow(ActiveTimeApplication activeTimeApplication);

        void ShowStatisticsWindow(ActiveTimeApplication activeTimeApplication);

        void ShowAboutWindow();
    }
}
