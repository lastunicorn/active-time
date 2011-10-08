using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.ActiveTime.Common
{
    public interface ITrayIconService
    {
        event EventHandler ExitClicked;
        event EventHandler ShowClicked;
        event EventHandler StartClicked;
        event EventHandler StopClicked;
        event EventHandler StopAndDeleteClicked;

        event EventHandler IconStateChanged;
        event EventHandler IconVisibleChanged;

        IconState IconState { get; set; }
        bool IconVisible { get; set; }
        bool StartEnabled { set; }
        bool StopEnabled { set; }

        void RaiseStopAndDeleteClicked();
        void RaiseStopClicked();
        void RaiseStartClicked();
        void RaiseExitClicked();
        void RaiseShowClicked();
    }
}
