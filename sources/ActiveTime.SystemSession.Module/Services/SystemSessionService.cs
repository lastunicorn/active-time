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

using System;
using DustInTheWind.ActiveTime.Common.Recording;
using Microsoft.Win32;

namespace DustInTheWind.ActiveTime.SystemSession.Module.Services
{
    /// <summary>
    /// This service monitors the Windows session and stops the Recorder Service when the user
    /// locks the session or logs off. When the user unlocks the session, the recorderService
    /// is started only if it was previously running.
    /// </summary>
    /// <remarks>
    /// This is an active service, that initiates actions.
    /// </remarks>
    internal class SystemSessionService
    {
        private readonly IRecorderService recorderService;

        /// <summary>
        /// Specifies if the Recorder was running when the session was locked.
        /// This value is used when the user unlocks the session.
        /// </summary>
        private bool recorderWasRunning;

        /// <summary>
        /// Initializes a new instance of <see cref="SystemSessionService"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public SystemSessionService(IRecorderService recorderService)
        {
            this.recorderService = recorderService ?? throw new ArgumentNullException(nameof(recorderService));

            SystemEvents.SessionSwitch += HandleSessionSwitch;
        }

        private void HandleSessionSwitch(object sender, SessionSwitchEventArgs e)
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
                case SessionSwitchReason.SessionLock:
                    HandleSystemSessionOff();
                    break;

                case SessionSwitchReason.SessionLogon:
                case SessionSwitchReason.SessionUnlock:
                    HandleSystemSessionOn();
                    break;
            }
        }

        /// <summary>
        /// The user left the desk.
        /// </summary>
        private void HandleSystemSessionOff()
        {
            bool recorderIsRunning = recorderService.State == RecorderState.Running;

            if (recorderIsRunning)
                recorderService.Stop();

            recorderWasRunning = recorderIsRunning;
        }

        /// <summary>
        /// The user returned to his desk.
        /// </summary>
        private void HandleSystemSessionOn()
        {
            if (recorderWasRunning)
                recorderService.Start();
        }
    }
}