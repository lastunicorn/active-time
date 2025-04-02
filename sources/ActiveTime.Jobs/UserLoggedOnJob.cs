// ActiveTime
// Copyright (C) 2011-2024 Dust in the Wind
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

using DustInTheWind.ActiveTime.Infrastructure.JobEngine;
using Microsoft.Win32;

namespace DustInTheWind.ActiveTime.Jobs;

public class UserLoggedOnJob : JobBase
{
    /// <summary>
    /// This value is used when the user unlocks the session and specifies if the
    /// Recorder was running when the session was locked.
    /// </summary>
    private bool recorderWasRunning;
    private readonly JobCollection jobCollection;

    public override string Id { get; } = "UserLoggedOn";

    public UserLoggedOnJob(JobCollection jobCollection)
    {
        this.jobCollection = jobCollection ?? throw new ArgumentNullException(nameof(jobCollection));

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
                StopRecorder().GetAwaiter().GetResult();
                break;

            case SessionSwitchReason.SessionLogon:
            case SessionSwitchReason.SessionUnlock:
                StartRecorder();
                break;
        }
    }

    private async Task StopRecorder()
    {
        // The user left the desk.

        IJob recorderJob = jobCollection.Get("Recorder");

        bool recorderIsRunning = (recorderJob.State == JobState.Running);

        if (recorderIsRunning)
            await recorderJob.Stop();

        recorderWasRunning = recorderIsRunning;
    }

    private void StartRecorder()
    {
        // The user returned to his desk.

        //TimeSpan? timeFromLastStop = recorderWasRunning ? this.CalculateTimeFromLastStop() : null;

        if (recorderWasRunning)
        {
            IJob recorderJob = jobCollection.Get("Recorder");
            recorderJob.Start();
        }

        //if (timeFromLastStop != null && timeFromLastStop < TimeSpan.FromMinutes(1))
        //{
        //    Dictionary<string, object> parameters = new Dictionary<string, object>
        //        {
        //            { "Text", "Really?\nDo you think you can trick me?\n\nMake a REAL pause." }
        //        };

        //    shellNavigator.Navigate(ShellNames.MessageShell, parameters);
        //}
    }
}
