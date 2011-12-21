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
    class PauseReminder
    {
        private IRecorderService recorderService;
        private IShellNavigator shellNavigator;
        private IReminder reminder;

        private TimeSpan maxWorkTime;

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

            maxWorkTime = TimeSpan.FromHours(1);

            recorderService.Started += new EventHandler(recorderService_Started);
            recorderService.Stopped += new EventHandler(recorderService_Stopped);
            reminder.Ring += new EventHandler<RingEventArgs>(reminder_Ring);
        }

        private void recorderService_Started(object sender, EventArgs e)
        {
            reminder.Start(maxWorkTime);
        }

        private void recorderService_Stopped(object sender, EventArgs e)
        {
            reminder.Stop();
        }

        private void reminder_Ring(object sender, RingEventArgs e)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Pause", "Make a pause.");
            shellNavigator.Navigate(ShellNames.MessageShell, parameters);
        }
    }
}
