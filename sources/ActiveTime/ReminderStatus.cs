using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.ActiveTime
{
    /// <summary>
    /// Represents the status of the <see cref="Reminder"/>.
    /// </summary>
    public enum ReminderStatus
    {
        /// <summary>
        /// The timer is not started. Or it was forced to stop.
        /// </summary>
        NotStarted,

        /// <summary>
        /// The clock is running and the Ring event has not been triggered yet.
        /// </summary>
        Running,

        /// <summary>
        /// The timer elapsed and the Ring event was raised, but the Reminder was
        /// not stoppd yet. It still may be snoozed.
        /// </summary>
        Stopping,

        /// <summary>
        /// The clock has rang, but it was snoozed.
        /// </summary>
        Snooze,

        /// <summary>
        /// The clock was elapsed and the Ring event has already been triggered.
        /// </summary>
        Finished
    }
}
