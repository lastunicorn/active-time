using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.ActiveTime.Main.Services
{
    class TrayIconService : ITrayIconService
    {
        #region IconState

        private IconState iconState;
        public IconState IconState
        {
            get { return iconState; }
            set
            {
                iconState = value;
                OnIconStateChanged(EventArgs.Empty);
            }
        }

        public event EventHandler IconStateChanged;

        protected virtual void OnIconStateChanged(EventArgs e)
        {
            if (IconStateChanged != null)
                IconStateChanged(this, e);
        }

        #endregion

        #region IconVisible

        private bool iconVisible;
        public bool IconVisible
        {
            get { return iconVisible; }
            set
            {
                iconVisible = value;
                OnIconVisibleChanged(EventArgs.Empty);
            }
        }

        public event EventHandler IconVisibleChanged;

        protected virtual void OnIconVisibleChanged(EventArgs e)
        {
            if (IconVisibleChanged != null)
                IconVisibleChanged(this, e);
        }

        #endregion


        public bool StartEnabled
        {
            set { }
        }

        public bool StopEnabled
        {
            set { }
        }

        public event EventHandler ExitClicked;

        protected virtual void OnExitClicked(EventArgs e)
        {
            if (ExitClicked != null)
                ExitClicked(this, e);
        }

        public event EventHandler ShowClicked;

        protected virtual void OnShowClicked(EventArgs e)
        {
            if (ShowClicked != null)
                ShowClicked(this, e);
        }

        public event EventHandler StartClicked;

        protected virtual void OnStartClicked(EventArgs e)
        {
            if (StartClicked != null)
                StartClicked(this, e);
        }

        public event EventHandler StopClicked;

        protected virtual void OnStopClicked(EventArgs e)
        {
            if (StopClicked != null)
                StopClicked(this, e);
        }

        public event EventHandler StopAndDeleteClicked;

        protected virtual void OnStopAndDeleteClicked(EventArgs e)
        {
            if (StopAndDeleteClicked != null)
                StopAndDeleteClicked(this, e);
        }


        public void RaiseStopAndDeleteClicked()
        {
            OnStopAndDeleteClicked(EventArgs.Empty);
        }

        public void RaiseStopClicked()
        {
            OnStopClicked(EventArgs.Empty);
        }

        public void RaiseStartClicked()
        {
            OnStartClicked(EventArgs.Empty);
        }

        public void RaiseExitClicked()
        {
            OnExitClicked(EventArgs.Empty);
        }

        public void RaiseShowClicked()
        {
            OnShowClicked(EventArgs.Empty);
        }
    }
}
