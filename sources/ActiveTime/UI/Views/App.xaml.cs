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
using System.Windows;
using System.Windows.Threading;
using DustInTheWind.ActiveTime.Recording;
using DustInTheWind.ActiveTime.UI;
using DustInTheWind.ActiveTime.UI.Views;
using DustInTheWind.ActiveTime.Watchman;
using Microsoft.Win32;

namespace DustInTheWind.ActiveTime
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ActiveTimeApplication activeTimeApplication;

        #region Recorder

        public void recorder_Started(object sender, EventArgs e)
        {
            if (trayIconManager != null)
            {
                trayIconManager.SetIconOn();
                trayIconManager.StartEnabled = false;
                trayIconManager.StopEnabled = true;

                //activeTimeApplication.Reminder.Start(TimeSpan.FromHours(1));
                activeTimeApplication.Reminder.Start(DustInTheWind.ActiveTime.Properties.Settings.Default.ReminderInterval);
            }
        }

        public void recorder_Stopped(object sender, EventArgs e)
        {
            if (trayIconManager != null)
            {
                trayIconManager.SetIconOff();
                trayIconManager.StartEnabled = true;
                trayIconManager.StopEnabled = false;

                activeTimeApplication.Reminder.Stop();
            }
        }

        #endregion

        private MainWindow window;

#if !DEBUG
        private Guard guard;
#endif

        #region Timer

        /// <summary>
        /// Timer used to update the current record's end Time.
        /// </summary>
        private DispatcherTimer timer;

        private void timer_Tick(object sender, EventArgs e)
        {
            if (activeTimeApplication.Recorder != null)
            {
                activeTimeApplication.Recorder.Stamp();
            }
        }

        #endregion

        #region Application

        protected override void OnStartup(StartupEventArgs e)
        {
            //WindowDisplayer.ShowWindow(new PauseWindow(), 1000, 10);

            bool stop = false;

#if !DEBUG
            try
            {
                // Ensure that the application is started only once on the current machine.
                guard = new Guard("DustInTheWind.ActiveTime", GuardLevel.Machine);
            }
            catch (ActiveTimeException)
            {
                stop = true;
                MessageBox.Show("The application is already started. Current instance will not start.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                stop = true;
                MessageBox.Show("Error creating the unique instance.\nInternal error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
#endif

            if (stop)
            {
                Shutdown();
            }
            else
            {
                activeTimeApplication = new ActiveTimeApplication();

                //TasksManager tasksManager = new TasksManager(WinformsViewsManager.
                //                                             GetDefaultConfig());
                //tasksManager.StartTask(typeof(MainTask));

                bool allowToContinue = true;

                if (!allowToContinue)
                {
                    Shutdown();
                }
                else
                {
                    trayIconManager = new TrayIconManager();
                    trayIconManager.ExitClicked += new EventHandler(trayIconManager_ExitClicked);
                    trayIconManager.ShowClicked += new EventHandler(trayIconManager_ShowClicked);
                    trayIconManager.StopClicked += new EventHandler(trayIconManager_StopClicked);
                    trayIconManager.StartClicked += new EventHandler(trayIconManager_StartClicked);
                    trayIconManager.StopAndDeleteClicked += new EventHandler(trayIconManager_StopAndDeleteClicked);

                    trayIconManager.ShowIcon();

                    Reminder reminder = activeTimeApplication.Reminder;
                    reminder.SnoozeTime = TimeSpan.FromMinutes(10);
                    reminder.Ring += new EventHandler<RingEventArgs>(reminder_Ring);

                    Recorder recorder = activeTimeApplication.Recorder;
                    recorder.Started += new EventHandler(recorder_Started);
                    recorder.Stopped += new EventHandler(recorder_Stopped);
                    recorder.Start();


                    SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);

                    timer = new DispatcherTimer();
                    timer.Tick += new EventHandler(timer_Tick);
                    timer.Interval = TimeSpan.FromMinutes(1);
                    timer.Start();

                    base.OnStartup(e);
                }
            }
        }

        private void reminder_Ring(object sender, RingEventArgs e)
        {
            try
            {
                Reminder reminder = sender as Reminder;
                string text = string.Format("Time passed: {0:hh\\:mm\\:ss}\n\nMake a pause NOW!", DateTime.Now - reminder.StartTime);


                this.Dispatcher.Invoke(new ShowPause(delegate
                {
                    PauseWindow pauseWindow = new PauseWindow(text);
                    pauseWindow.ShowDialog();
                }));

                e.Snooze = true;
                e.SnoozeTime = DustInTheWind.ActiveTime.Properties.Settings.Default.SnoozeInterval;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private delegate void ShowPause();

        protected override void OnLoadCompleted(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (trayIconManager != null)
            {
                trayIconManager.HideIcon();
            }

            if (activeTimeApplication != null)
            {
                if (activeTimeApplication.Recorder != null)
                {
                    activeTimeApplication.Recorder.Stop();
                    activeTimeApplication.Recorder.Dispose();
                }
            }

            base.OnExit(e);
        }

        #endregion

        #region Tray Icon

        private TrayIconManager trayIconManager;

        private void trayIconManager_ShowClicked(object sender, EventArgs e)
        {
            try
            {
                if (window == null)
                {
                    window = new MainWindow(activeTimeApplication);
                    window.TrayIconManager = trayIconManager;
                }

                window.Show();

                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;

                window.Activate();
                window.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void trayIconManager_StartClicked(object sender, EventArgs e)
        {
            activeTimeApplication.Recorder.Start();
        }

        private void trayIconManager_StopClicked(object sender, EventArgs e)
        {
            activeTimeApplication.Recorder.Stop();
        }

        private void trayIconManager_StopAndDeleteClicked(object sender, EventArgs e)
        {
            activeTimeApplication.Recorder.StopAndDeleteLastRecord();
        }

        private void trayIconManager_ExitClicked(object sender, EventArgs e)
        {
            if (window != null)
            {
                window.AllowClose = true;
                window.Close();
            }

            Shutdown();
        }

        #endregion


        private void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
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
                    activeTimeApplication.Recorder.Stop();
                    break;

                case SessionSwitchReason.SessionLogon:
                case SessionSwitchReason.SessionUnlock:
                    // The user returned to his desk.

                    TimeSpan? timeFromLastStop = activeTimeApplication.Recorder.TimeFromLastStop();

                    activeTimeApplication.Recorder.Start();

                    if (timeFromLastStop < TimeSpan.FromSeconds(20))
                    {
                        PauseWindow pauseWindow = new PauseWindow("Really?\nDo you think you can trick me?\n\nMake a REAL pause.");
                        pauseWindow.ShowDialog();
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
