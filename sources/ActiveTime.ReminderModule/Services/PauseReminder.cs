// ActiveTime
// Copyright (C) 2011 Dust in the Wind
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
using System.Collections.Generic;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.UI;
using DustInTheWind.ActiveTime.Common.UI.ShellNavigation;
using DustInTheWind.ActiveTime.ReminderModule.Reminding;

namespace DustInTheWind.ActiveTime.ReminderModule.Services
{
    /// <summary>
    /// The recorder is watched and a notification is displayed to the user when the
    /// <see cref="PauseInterval"/> is elapsed.
    /// </summary>
    internal class PauseReminder : IPauseReminder
    {
        /// <summary>
        /// The recorder is used to check when the user is working.
        /// </summary>
        private readonly IRecorderService recorderService;

        /// <summary>
        /// It is used to display a message box to the user.
        /// </summary>
        private readonly IShellNavigator shellNavigator;

        /// <summary>
        /// It is used to calculate the time when the user should make a pause.
        /// </summary>
        private readonly IReminder reminder;

        private bool isMonitoring;

        /// <summary>
        /// The interval time after which the "pause" message should be displayed.
        /// This value should be set before calling the <see cref="M:StartMonitoring"/> method.
        /// </summary>
        public TimeSpan PauseInterval { get; set; }

        /// <summary>
        /// The interval time after which the "snooze" message should be displayed.
        /// </summary>
        public TimeSpan SnoozeInterval { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PauseReminder"/> class.
        /// </summary>
        /// <param name="recorderService">The recorder is used to check when the user is working.</param>
        /// <param name="shellNavigator">It is used to display a message box to the user.</param>
        /// <param name="reminder">It is used to calculate the time when the user should make a pause.</param>
        public PauseReminder(IRecorderService recorderService, IShellNavigator shellNavigator, IReminder reminder)
        {
            if (recorderService == null)
                throw new ArgumentNullException("recorderService");

            if (shellNavigator == null)
                throw new ArgumentNullException("shellNavigator");

            if (reminder == null)
                throw new ArgumentNullException("reminder");

            this.recorderService = recorderService;
            this.shellNavigator = shellNavigator;
            this.reminder = reminder;

            PauseInterval = TimeSpan.FromHours(1);
            SnoozeInterval = TimeSpan.FromMinutes(3);
        }

        public void StartMonitoring()
        {
            if (isMonitoring)
                return;

            recorderService.Started += HandleRecorderServiceStarted;
            recorderService.Stopped += HandleRecorderServiceStopped;
            reminder.Ring += HandleReminderRing;

            if (recorderService.State == RecorderState.Running)
                reminder.Start(PauseInterval);

            isMonitoring = true;
        }

        private void HandleRecorderServiceStarted(object sender, EventArgs e)
        {
            reminder.Start(PauseInterval);
        }

        private void HandleRecorderServiceStopped(object sender, EventArgs e)
        {
            reminder.Stop();
        }

        private void HandleReminderRing(object sender, RingEventArgs e)
        {
            TimeSpan timeFromLastPause = DateTime.Now - reminder.StartTime;

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "Text", string.Format("Time passed: {0:hh\\:mm\\:ss}\n\nMake a pause NOW!", timeFromLastPause) }
            };

            shellNavigator.Navigate(ShellNames.MessageShell, parameters);

            e.Snooze = true;
            e.SnoozeTime = SnoozeInterval;
        }
    }
}
