// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;

namespace DustInTheWind.ActiveTime.ReminderModule.Reminding
{
    /// <summary>
    /// Provides data for <see cref="IReminder.Ring"/> event.
    /// </summary>
    public class RingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value that specifies if the timer should be delayed.
        /// The <see cref="SnoozeTime"/> property specifies the time of the delay.
        /// If the <see cref="SnoozeTime"/> value is not set, the default time of
        /// the <see cref="IReminder"/> is used.
        /// </summary>
        public bool Snooze { get; set; }

        /// <summary>
        /// Gets or sets the time used to delay the ring.
        /// If this value is not set, the default time of
        /// the <see cref="IReminder"/> is used.
        /// </summary>
        public TimeSpan? SnoozeTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RingEventArgs"/> class.
        /// </summary>
        public RingEventArgs()
        {
            Snooze = false;
            SnoozeTime = null;
        }
    }
}
