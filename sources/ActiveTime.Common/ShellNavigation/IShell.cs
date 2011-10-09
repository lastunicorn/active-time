using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.ActiveTime.Common.ShellNavigation
{
    public interface IShell
    {
        Dictionary<string, object> NavigationParameters { get; set; }
    }
}
