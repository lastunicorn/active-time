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
using System.Timers;
using Microsoft.Lync.Model;

namespace DustInTheWind.ActiveTime.ReminderModule.LyncInhibitor
{
    internal class AvailabilityWatcher
    {
        private readonly LyncClient client;
        private ContactAvailability currentAvailability;

        public ContactAvailability CurrentAvailability
        {
            get { return currentAvailability; }
            private set
            {
                currentAvailability = value;
                OnAvailabilityChanged();
            }
        }

        public event EventHandler AvailabilityChanged;

        public AvailabilityWatcher()
        {
            client = LyncClient.GetClient();

            if (client.InSuppressedMode)
                throw new Exception("Lync UI is suppressed.");
            
            currentAvailability = RetrieveAvailability();

            client.StateChanged += HandleClientStateChanged;

            StartTimer();
        }

        private void HandleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            ContactAvailability newAvailability = RetrieveAvailability();

            if (newAvailability != currentAvailability)
                CurrentAvailability = newAvailability;
        }

        private void HandleClientStateChanged(object sender, ClientStateChangedEventArgs e)
        {
            CurrentAvailability = RetrieveAvailability();
        }

        private void StartTimer()
        {
            Timer timer = new Timer
            {
                Interval = 3000,
                AutoReset = true
            };

            timer.Elapsed += HandleTimerElapsed;

            timer.Start();
        }

        private ContactAvailability RetrieveAvailability()
        {
            object contactInformation = client.Self?.Contact?.GetContactInformation(ContactInformationType.Availability) ?? ContactAvailability.None;
            return (ContactAvailability)contactInformation;
        }

        protected virtual void OnAvailabilityChanged()
        {
            AvailabilityChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}