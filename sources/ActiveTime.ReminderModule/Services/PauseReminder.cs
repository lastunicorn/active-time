using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Recording;
using DustInTheWind.ActiveTime.Common.ShellNavigation;
using DustInTheWind.ActiveTime.Common.Reminding;

namespace DustInTheWind.ActiveTime.ReminderModule.Services
{
    class PauseReminder : IPauseReminder
    {
        private IRecorderService recorderService;
        private IShellNavigator shellNavigator;
        private IReminder reminder;

        private bool isMonitoring = false;

        public TimeSpan PauseInterval { get; set; }

        public TimeSpan SnoozeInterval { get; set; }

        public PauseReminder(IRecorderService recorderService, IShellNavigator shellNavigator, IReminder reminder)
        {
            this.reminder = reminder;
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
            //PauseInterval = TimeSpan.FromSeconds(5);
            SnoozeInterval = TimeSpan.FromMinutes(3);
        }

        public void StartMonitoring()
        {
            if (isMonitoring) return;

            recorderService.Started += new EventHandler(recorderService_Started);
            recorderService.Stopped += new EventHandler(recorderService_Stopped);
            reminder.Ring += new EventHandler<RingEventArgs>(reminder_Ring);

            if (recorderService.State == RecorderState.Running)
                reminder.Start(PauseInterval);
        }

        private void recorderService_Started(object sender, EventArgs e)
        {
            reminder.Start(PauseInterval);
        }

        private void recorderService_Stopped(object sender, EventArgs e)
        {
            reminder.Stop();
        }

        private void reminder_Ring(object sender, RingEventArgs e)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Text", "Make a pause.");
            shellNavigator.Navigate(ShellNames.MessageShell, parameters);

            e.Snooze = true;
            e.SnoozeTime = SnoozeInterval;
        }
    }
}
