using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows;
using Microsoft.Practices.Prism.Events;
using DustInTheWind.ActiveTime.Common;
using DustInTheWind.ActiveTime.Common.Events;
using DustInTheWind.ActiveTime.Common.Recording;

namespace DustInTheWind.ActiveTime.TrayIconModule.ViewModels
{
    class TrayIconPresenter
    {
        private ITrayIconView view;
        public ITrayIconView View
        {
            get { return view; }
            set
            {
                view = value;
                if (view != null) Initialize();
            }
        }

        private Icon iconOn;
        private Icon iconOff;

        private IEventAggregator eventAggregator;
        private IRecorder recorder;
        private IApplicationService applicationService;

        public TrayIconPresenter(IRecorder recorder, IEventAggregator eventAggregator, IApplicationService applicationService)
        {
            if (recorder == null)
                throw new ArgumentNullException("recorder");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            if (applicationService == null)
                throw new ArgumentNullException("applicationService");

            this.recorder = recorder;
            this.eventAggregator = eventAggregator;
            this.applicationService = applicationService;

            iconOn = Properties.Resources.tray_on;
            iconOff = Properties.Resources.tray_off;

            ApplicationExitEvent applicationExitEvent = eventAggregator.GetEvent<ApplicationExitEvent>();
            applicationExitEvent.Subscribe(OnApplicationExitEvent);

            //recorder.IsStartedChanged += new EventHandler(recorder_IsStartedChanged);
            recorder.Started += new EventHandler(recorder_Started);
            recorder.Stopped += new EventHandler(recorder_Stopped);
        }

        void recorder_Stopped(object sender, EventArgs e)
        {
            RefreshView();
        }

        void recorder_Started(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void OnApplicationExitEvent(object o)
        {
            Hide();
        }

        //void recorder_IsStartedChanged(object sender, EventArgs e)
        //{
        //    RefreshView();
        //}

        private void RefreshView()
        {
            switch (recorder.State)
            {
                case RecorderState.Stopped:
                    SetIconOff();
                    SetStartMenuItemEnabled();
                    SetStopMenuItemDisabled();
                    break;

                case RecorderState.Running:
                    SetIconOn();
                    SetStartMenuItemDisabled();
                    SetStopMenuItemEnabled();
                    break;

                default:
                    break;
            }
        }

        private void Initialize()
        {
            RefreshView();
            Show();
        }

        private void SetIconOn()
        {
            if (View != null)
                View.Icon = iconOn;
        }

        private void SetIconOff()
        {
            if (View != null)
                View.Icon = iconOff;
        }

        private void Show()
        {
            if (View != null)
                View.Visible = true;
        }

        private void Hide()
        {
            if (View != null)
                View.Visible = false;
        }

        private void SetStopMenuItemEnabled()
        {
            if (View != null)
                View.StopMenuItemEnabled = true;
        }

        private void SetStopMenuItemDisabled()
        {
            if (View != null)
                View.StopMenuItemEnabled = false;
        }

        private void SetStartMenuItemEnabled()
        {
            if (View != null)
                View.StartMenuItemEnabled = true;
        }

        private void SetStartMenuItemDisabled()
        {
            if (View != null)
                View.StartMenuItemEnabled = false;
        }

        public void StopAndDeleteClicked()
        {
            recorder.Stop(true);
        }

        public void StopClicked()
        {
            recorder.Stop();
        }

        public void StartClicked()
        {
            recorder.Start();
        }

        public void ExitClicked()
        {
            applicationService.Exit();
        }

        public void ShowClicked()
        {
            applicationService.ShowMainWindow();
        }

        public void LeftDoubleClicked()
        {
            applicationService.ShowMainWindow();
        }
    }
}
