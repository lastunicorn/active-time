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
using DustInTheWind.ActiveTime.Common.Recording;

namespace DustInTheWind.ActiveTime.Commands
{
    internal class StopRecorderCommand : ICommand
    {
        private readonly IRecorderService recorder;
        public event EventHandler CanExecuteChanged;

        public StopRecorderCommand(IRecorderService recorder)
        {
            if (recorder == null) throw new ArgumentNullException(nameof(recorder));
            this.recorder = recorder;

            recorder.Started += HandleRecorderStarted;
            recorder.Stopped += HandleRecorderStopped;
        }

        private void HandleRecorderStarted(object sender, EventArgs eventArgs)
        {
            OnCanExecuteChanged();
        }

        private void HandleRecorderStopped(object sender, EventArgs eventArgs)
        {
            OnCanExecuteChanged();
        }

        public bool CanExecute(object parameter)
        {
            return recorder.State == RecorderState.Running;
        }

        public void Execute(object parameter)
        {
            recorder.Stop();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}