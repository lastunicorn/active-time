using System.Collections.Generic;

namespace DustInTheWind.ActiveTime.Common.ShellNavigation
{
    public interface IShell
    {
        Dictionary<string, object> NavigationParameters { get; set; }
    }
}
