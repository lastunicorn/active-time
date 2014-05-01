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
using System.Windows.Input;
using DustInTheWind.ActiveTime.Common.UI;
using Microsoft.Practices.Prism.Commands;

namespace DustInTheWind.ActiveTime.MainGuiModule.ViewModels
{
    public class ButtonBarViewModel : ViewModelBase
    {
        public enum ButtonBarDataState
        {
            NoData,
            SavedData,
            UnsavedData
        }

        public ICommand ApplyCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        private bool isApplyButtonEnabled;
        public bool IsApplyButtonEnabled
        {
            get { return isApplyButtonEnabled; }
            set
            {
                isApplyButtonEnabled = value;
                NotifyPropertyChanged("IsApplyButtonEnabled");
            }
        }

        private bool isCancelButtonEnabled;
        public bool IsCancelButtonEnabled
        {
            get { return isCancelButtonEnabled; }
            set
            {
                isCancelButtonEnabled = value;
                NotifyPropertyChanged("IsCancelButtonEnabled");
            }
        }

        private bool isSaveButtonEnabled;
        public bool IsSaveButtonEnabled
        {
            get { return isSaveButtonEnabled; }
            set
            {
                isSaveButtonEnabled = value;
                NotifyPropertyChanged("IsSaveButtonEnabled");
            }
        }

        private ButtonBarDataState dataState;
        public ButtonBarDataState DataState
        {
            get { return dataState; }
            set
            {
                switch (value)
                {
                    default:
                    case ButtonBarDataState.NoData:
                        IsApplyButtonEnabled = false;
                        IsCancelButtonEnabled = true;
                        IsSaveButtonEnabled = false;
                        break;

                    case ButtonBarDataState.SavedData:
                        IsApplyButtonEnabled = false;
                        IsCancelButtonEnabled = true;
                        IsSaveButtonEnabled = true;
                        break;

                    case ButtonBarDataState.UnsavedData:
                        IsApplyButtonEnabled = true;
                        IsCancelButtonEnabled = true;
                        IsSaveButtonEnabled = true;
                        break;
                }

                dataState = value;

                CommandManager.InvalidateRequerySuggested();
            }
        }

        #region Event ApplyButtonClicked

        /// <summary>
        /// Event raised when the apply button was clicked.
        /// </summary>
        public event EventHandler ApplyButtonClicked;
        
        /// <summary>
        /// Raises the <see cref="ApplyButtonClicked"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected virtual void OnApplyButtonClicked(EventArgs e)
        {
            if (ApplyButtonClicked != null)
            {
                ApplyButtonClicked(this, e);
            }
        }

        #endregion

        #region Event CancelButtonClicked

        /// <summary>
        /// Event raised when the apply button was clicked.
        /// </summary>
        public event EventHandler CancelButtonClicked;
        
        /// <summary>
        /// Raises the <see cref="CancelButtonClicked"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected virtual void OnCancelButtonClicked(EventArgs e)
        {
            if (CancelButtonClicked != null)
            {
                CancelButtonClicked(this, e);
            }
        }

        #endregion

        #region Event SaveButtonClicked

        /// <summary>
        /// Event raised when the apply button was clicked.
        /// </summary>
        public event EventHandler SaveButtonClicked;
        
        /// <summary>
        /// Raises the <see cref="SaveButtonClicked"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected virtual void OnSaveButtonClicked(EventArgs e)
        {
            if (SaveButtonClicked != null)
            {
                SaveButtonClicked(this, e);
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonBarViewModel"/> class.
        /// </summary>
        public ButtonBarViewModel()
        {
            ApplyCommand = new DelegateCommand(OnApplyCommandExecuted, () => IsApplyButtonEnabled);
            CancelCommand = new DelegateCommand(OnCancelCommandExecuted, () => isCancelButtonEnabled);
            SaveCommand = new DelegateCommand(OnSaveCommandExecuted, () => isSaveButtonEnabled);

            DataState = ButtonBarDataState.NoData;
        }

        private void OnApplyCommandExecuted()
        {
            OnApplyButtonClicked(EventArgs.Empty);
        }

        private void OnCancelCommandExecuted()
        {
            OnCancelButtonClicked(EventArgs.Empty);
        }

        private void OnSaveCommandExecuted()
        {
            OnSaveButtonClicked(EventArgs.Empty);
        }
    }
}
