using DustInTheWind.ActiveTime.UI.Models;

namespace DustInTheWind.ActiveTime.UI.IViews
{
    interface ICommentsView : IViewBase
    {
        CommentsModel Model { set; }

        void CloseWithCancel();

        void CloseWithOk();
    }
}
