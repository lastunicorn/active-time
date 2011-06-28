using System;

namespace DustInTheWind.ActiveTime.UI.IViews
{
    public interface IViewBase
    {
        void DisplayError(Exception ex);
        void DisplayInfoMessage(string message);
        void DisplayErrorMessage(string message);
    }
}
