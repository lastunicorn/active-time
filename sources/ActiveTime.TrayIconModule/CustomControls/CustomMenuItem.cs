// ActiveTime
// Copyright (C) 2011-2017 Dust in the Wind
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
using System.Windows.Forms;
using System.Windows.Input;

namespace DustInTheWind.ActiveTime.TrayIconModule.CustomControls
{
    internal class CustomMenuItem : ToolStripMenuItem
    {
        private ICommand command;

        public ICommand Command
        {
            get { return command; }
            set
            {
                if (command != null)
                {
                    command.CanExecuteChanged -= HandleCommandCanExecuteChanged;
                    Enabled = true;
                }

                command = value;

                if (command != null)
                {
                    command.CanExecuteChanged += HandleCommandCanExecuteChanged;
                    Enabled = command.CanExecute(CommandParameter);
                }
            }
        }

        public bool CommandParameter { get; set; }

        private void HandleCommandCanExecuteChanged(object sender, EventArgs e)
        {
            Enabled = command.CanExecute(CommandParameter);
        }
        
        protected override void OnClick(EventArgs e)
        {
            command?.Execute(CommandParameter);

            base.OnClick(e);
        }
    }
}
