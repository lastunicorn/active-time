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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace DustInTheWind.ActiveTime.UI
{
    class WindowDisplayer
    {
        public static void ShowWindow(Window window)
        {
            window.Show();
        }

        public static void ShowWindow(Window window, int interval, int steps)
        {
            Task task = new Task(new Action(delegate()
            {
                float stepInterval = interval / steps;

                window.Dispatcher.Invoke(new SetInt(delegate(int value) { }), null);
                window.Opacity = 0;
                window.Show();

                do
                {
                    window.Opacity += 0.1;
                    Thread.Sleep((int)stepInterval);
                }
                while (window.Opacity < 1);
            }));

            task.Start();
        }

        private delegate void SetInt(int value);

        public static bool? ShowDialogWindow(Window window)
        {
            return window.ShowDialog();
        }
    }
}
