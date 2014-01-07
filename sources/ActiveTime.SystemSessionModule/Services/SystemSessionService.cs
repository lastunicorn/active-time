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
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.ShellNavigation;
using Microsoft.Win32;

namespace DustInTheWind.ActiveTime.SystemSessionModule.Services
{
    /// <summary>
    /// This service monitors the Windows session and stops the recorder when the user
    /// locks the session or logs off. When the user unlocks the session, the recorder
    /// is started only if it was previously running.
    /// </summary>
    /// <remarks>
    /// This is an active service, that initiates actions.
    /// </remarks>
    class SystemSessionService
    {
        private readonly IRecorderService recorder;
        private readonly IShellNavigator navigator;

        /// <summary>
        /// This value is used when the user unlocks the session and specifies if the
        /// Recorder was running when the session was locked.
        /// </summary>
        private bool recorderWasRunning = false;

        /// <summary>
        /// Initializes a new instance of <see cref="SystemSessionService"/> class.
        /// </summary>
        /// <param name="recorder"></param>
        /// <param name="navigator"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SystemSessionService(IRecorderService recorder, IShellNavigator navigator)
        {
            if (recorder == null)
                throw new ArgumentNullException("recorder");

            if (navigator == null)
                throw new ArgumentNullException("navigator");

            this.recorder = recorder;
            this.navigator = navigator;

            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.ConsoleConnect:
                    break;

                case SessionSwitchReason.ConsoleDisconnect:
                    break;

                case SessionSwitchReason.RemoteConnect:
                    break;

                case SessionSwitchReason.RemoteDisconnect:
                    break;

                case SessionSwitchReason.SessionRemoteControl:
                    break;

                case SessionSwitchReason.SessionLogoff:
                    break;

                case SessionSwitchReason.SessionLock:
                    StopRecorder();
                    break;

                case SessionSwitchReason.SessionLogon:
                    break;

                case SessionSwitchReason.SessionUnlock:
                    StartRecorder();
                    break;
            }
        }

        private void StopRecorder()
        {
            // The user left the desk.

            if (recorder.State == RecorderState.Running)
            {
                recorderWasRunning = true;
                recorder.Stop();
            }
            else
            {
                recorderWasRunning = false;
            }
        }

        private void StartRecorder()
        {
            // The user returned to his desk.

            TimeSpan? timeFromLastStop = recorderWasRunning ? recorder.GetTimeFromLastStop() : null;

            if (recorderWasRunning)
                recorder.Start();

            if (timeFromLastStop != null && timeFromLastStop < TimeSpan.FromMinutes(1))
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "Text", "Really?\nDo you think you can trick me?\n\nMake a REAL pause." }
                };

                navigator.Navigate(ShellNames.MessageShell, parameters);
            }
        }
    }
}
