using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Reminding;
using DustInTheWind.ActiveTime.Reminding.Services;

namespace DustInTheWind.ActiveTime.MainModule.Services
{
    class MainService : IMainService
    {
        private readonly IRecorder recorder;
        private readonly IReminder reminder;

        public MainService(IRecorder recorder, IReminder reminder)
        {
            if (recorder == null)
                throw new ArgumentNullException("recorder");

            if (reminder == null)
                throw new ArgumentNullException("reminder");

            this.recorder = recorder;
            this.reminder = reminder;

            reminder.SnoozeTime = TimeSpan.FromMinutes(10);
            reminder.Ring += new EventHandler<RingEventArgs>(reminder_Ring);

            recorder.Started += new EventHandler(recorder_Started);
            recorder.Stopped += new EventHandler(recorder_Stopped);
            recorder.Start();
        }

        private void recorder_Started(object sender, EventArgs e)
        {
            reminder.Start(DustInTheWind.ActiveTime.Properties.Settings.Default.ReminderInterval);
        }

        private void recorder_Stopped(object sender, EventArgs e)
        {
            reminder.Stop();
        }

        private void reminder_Ring(object sender, RingEventArgs e)
        {
            try
            {
                Reminder reminder = sender as Reminder;
                string text = string.Format("Time passed: {0:hh\\:mm\\:ss}\n\nMake a pause NOW!", DateTime.Now - reminder.StartTime);

                // Display Pause Window

                //this.Dispatcher.Invoke(new ShowPause(delegate
                //{
                //    PauseWindow pauseWindow = new PauseWindow(text);
                //    pauseWindow.ShowDialog();
                //}));

                e.Snooze = true;
                e.SnoozeTime = DustInTheWind.ActiveTime.Properties.Settings.Default.SnoozeInterval;
            }
            catch (Exception ex)
            {
                // Display Error window
                //MessageBox.Show(ex.ToString());
            }
        }

        public void Start()
        {
            
        }
    }
}
