using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Reminding;
using DustInTheWind.ActiveTime.Reminding.Services;

namespace DustInTheWind.ActiveTime.Main.Services
{
    class MainService : IMainService
    {
        private readonly IRecorder recorder;
        private readonly IReminder reminder;
        private readonly ITrayIconService trayIconService;

        public MainService(IRecorder recorder, IReminder reminder, ITrayIconService trayIconService)
        {
            if (recorder == null)
                throw new ArgumentNullException("recorder");

            if (reminder == null)
                throw new ArgumentNullException("reminder");

            if (trayIconService == null)
                throw new ArgumentNullException("trayIconService");

            this.recorder = recorder;
            this.reminder = reminder;
            this.trayIconService = trayIconService;

            trayIconService.IconState = IconState.Off;
            trayIconService.IconVisible = true;

            reminder.SnoozeTime = TimeSpan.FromMinutes(10);
            reminder.Ring += new EventHandler<RingEventArgs>(reminder_Ring);

            recorder.Started += new EventHandler(recorder_Started);
            recorder.Stopped += new EventHandler(recorder_Stopped);
            recorder.Start();
        }

        private void recorder_Started(object sender, EventArgs e)
        {
            trayIconService.IconState = IconState.On;
            trayIconService.StartEnabled = false;
            trayIconService.StopEnabled = true;

            reminder.Start(DustInTheWind.ActiveTime.Properties.Settings.Default.ReminderInterval);
        }

        private void recorder_Stopped(object sender, EventArgs e)
        {

            trayIconService.IconState = IconState.Off;
            trayIconService.StartEnabled = true;
            trayIconService.StopEnabled = false;

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
