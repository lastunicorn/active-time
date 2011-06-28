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
