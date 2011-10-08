using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DustInTheWind.ActiveTime.Common
{
    public interface ITrayIconView
    {
        Icon Icon { set; }
        bool Visible { set; }

        bool StartMenuItemEnabled { set; }
        bool StopMenuItemEnabled { set; }
    }
}
