// ActiveTime
// Copyright (C) 2011-2020 Dust in the Wind
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

namespace DustInTheWind.ActiveTime.Reminder.Module.Reminding
{
    /// <summary>
    /// Represents the status of the <see cref="IReminder"/>.
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
        /// not stopped yet. It still may be snoozed.
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
