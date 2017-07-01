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

using DustInTheWind.ActiveTime.ReminderModule.Inhibitors;
using Microsoft.Lync.Model;

namespace DustInTheWind.ActiveTime.ReminderModule.LyncInhibitor
{
    /// <summary>
    /// Stopps the reminder to display messages to the user if the he is not Free on Lync messenger.
    /// </summary>
    internal class LyncReminderInhibitor : IReminderInhibitor
    {
        private readonly AvailabilityWatcher availabilityWatcher;

        public bool Allow
        {
            get
            {
                ContactAvailability availability = availabilityWatcher.CurrentAvailability;

                return availability != ContactAvailability.Busy &&
                    availability != ContactAvailability.BusyIdle &&
                    availability != ContactAvailability.DoNotDisturb;
            }
        }

        public LyncReminderInhibitor()
        {
            availabilityWatcher = new AvailabilityWatcher();
        }
    }
}
