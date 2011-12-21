using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.ActiveTime.Common.Reminding
{
    public interface IPauseReminder
    {
        TimeSpan PauseInterval { get; set; }
        TimeSpan SnoozeInterval { get; set; }
        void StartMonitoring();
    }
}
