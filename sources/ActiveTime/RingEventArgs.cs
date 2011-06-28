using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.ActiveTime
{
    /// <summary>
    /// Provides data for <see cref="Ring"/> event.
    /// </summary>
    public class RingEventArgs : EventArgs
    {
        /// <summary>
        /// Specifies if the timer should be delayed.
        /// </summary>
        private bool snooze;

        /// <summary>
        /// Gets or sets a value that specifies if the timer should be delayed.
        /// The <see cref="P:SnoozeTime"/> property specifies the time of the delay.
        /// If the <see cref="P:SnoozeTime"/> value is not set, the default time of
        /// the <see cref="Reminder"/> is used.
        /// </summary>
        public bool Snooze
        {
            get { return snooze; }
            set { snooze = value; }
        }

        /// <summary>
        /// The time used to delay the ring.
        /// </summary>
        private TimeSpan? snoozeTime;

        /// <summary>
        /// Gets or sets the time used to delay the ring.
        /// If this value is not set, the default time of
        /// the <see cref="Reminder"/> is used.
        /// </summary>
        public TimeSpan? SnoozeTime
        {
            get { return snoozeTime; }
            set { snoozeTime = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RingEventArgs"/> class.
        /// </summary>
        public RingEventArgs()
            : base()
        {
            snooze = false;
            snoozeTime = null;
        }
    }
}
