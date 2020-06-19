using System;
using System.Windows.Forms;

namespace DustInTheWind.ActiveTime.MouseShaker.UserActivityMonitor
{
    /// <summary>
    /// Provides data for the MouseClickExt and MouseMoveExt events. It also provides a property Handled.
    /// Set this property to <b>true</b> to prevent further processing of the event in other applications.
    /// </summary>
    public class MouseEventExtArgs : MouseEventArgs
    {
        /// <summary>
        /// Set this property to <b>true</b> inside your event handler to prevent further processing of the event in other applications.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class. 
        /// </summary>
        public MouseEventExtArgs(MouseEventInfo mouseEventInfo)
            : base(mouseEventInfo.Button, mouseEventInfo.ClickCount, mouseEventInfo.X, mouseEventInfo.Y, mouseEventInfo.MouseWheelDelta)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class. 
        /// </summary>
        /// <param name="e">An ordinary <see cref="MouseEventArgs"/> argument to be extended.</param>
        internal MouseEventExtArgs(MouseEventArgs e)
            : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        { }
    }
}
