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
using System.Linq;
using System.Text;
using Microsoft.Win32;
using DustInTheWind.ActiveTime.Common;

namespace DustInTheWind.ActiveTime.MainModule.Services
{
    class SystemSessionService
    {
        private readonly IRecorder recorder;
        private readonly IShellNavigator navigator;

        private bool checkPauseLength = false;

        /// <summary>
        /// Initializes a new instance of <see cref="SystemSessionService"/> class.
        /// </summary>
        /// <param name="recorder"></param>
        public SystemSessionService(IRecorder recorder, IShellNavigator navigator)
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
                case SessionSwitchReason.SessionLock:
                    // The user left the desk.
                    if (recorder.State == Common.Recording.RecorderState.Running)
                    {
                        checkPauseLength = true;
                        recorder.Stop();
                    }
                    else
                    {
                        checkPauseLength = false;
                    }
                    break;

                case SessionSwitchReason.SessionLogon:
                case SessionSwitchReason.SessionUnlock:
                    // The user returned to his desk.

                    TimeSpan? timeFromLastStop = checkPauseLength ? recorder.GetTimeFromLastStop() : null;

                    recorder.Start();

                    if (timeFromLastStop != null && timeFromLastStop < TimeSpan.FromMinutes(1))
                    {
                        //navigator.DisplayMessageWindow("Really?\nDo you think you can trick me?\n\nMake a REAL pause.");
                        navigator.Navigate(ShellNames.MessageShell);
                        
                        //PauseWindow pauseWindow = new PauseWindow("Really?\nDo you think you can trick me?\n\nMake a REAL pause.");
                        //pauseWindow.ShowDialog();
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
