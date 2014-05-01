using System;

namespace DustInTheWind.ActiveTime.ReminderModule.Reminding
{
    public interface IPauseReminder
    {
        TimeSpan PauseInterval { get; set; }
        TimeSpan SnoozeInterval { get; set; }
        void StartMonitoring();
    }
}
