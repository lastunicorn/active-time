using System;
using System.Windows.Forms;
using DustInTheWind.ActiveTime.MouseShaker.WindowsApi;

namespace DustInTheWind.ActiveTime.MouseShaker.UserActivityMonitor
{

    /// <summary>
    /// This class monitors all mouse activities globally (also outside of the application) 
    /// and provides appropriate events.
    /// </summary>
    public static partial class HookManager
    {
        //################################################################
        #region Mouse events

        private static event MouseEventHandler mouseMove;

        /// <summary>
        /// Occurs when the mouse pointer is moved. 
        /// </summary>
        public static event MouseEventHandler MouseMove
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                mouseMove += value;
            }

            remove
            {
                mouseMove -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event EventHandler<MouseEventExtArgs> mouseMoveExt;

        /// <summary>
        /// Occurs when the mouse pointer is moved. 
        /// </summary>
        /// <remarks>
        /// This event provides extended arguments of type <see cref="MouseEventArgs"/> enabling you to 
        /// suppress further processing of mouse movement in other applications.
        /// </remarks>
        public static event EventHandler<MouseEventExtArgs> MouseMoveExt
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                mouseMoveExt += value;
            }

            remove
            {

                mouseMoveExt -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler mouseClick;

        /// <summary>
        /// Occurs when a click was performed by the mouse. 
        /// </summary>
        public static event MouseEventHandler MouseClick
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                mouseClick += value;
            }
            remove
            {
                mouseClick -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event EventHandler<MouseEventExtArgs> mouseClickExt;

        /// <summary>
        /// Occurs when a click was performed by the mouse. 
        /// </summary>
        /// <remarks>
        /// This event provides extended arguments of type <see cref="MouseEventArgs"/> enabling you to 
        /// suppress further processing of mouse click in other applications.
        /// </remarks>
        public static event EventHandler<MouseEventExtArgs> MouseClickExt
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                mouseClickExt += value;
            }
            remove
            {
                mouseClickExt -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler mouseDown;

        /// <summary>
        /// Occurs when the mouse a mouse button is pressed. 
        /// </summary>
        public static event MouseEventHandler MouseDown
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                mouseDown += value;
            }
            remove
            {
                mouseDown -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler mouseUp;

        /// <summary>
        /// Occurs when a mouse button is released. 
        /// </summary>
        public static event MouseEventHandler MouseUp
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                mouseUp += value;
            }
            remove
            {
                mouseUp -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler mouseWheel;

        /// <summary>
        /// Occurs when the mouse wheel moves. 
        /// </summary>
        public static event MouseEventHandler MouseWheel
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                mouseWheel += value;
            }
            remove
            {
                mouseWheel -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }


        private static event MouseEventHandler mouseDoubleClick;

        //The double click event will not be provided directly from hook.
        //To fire the double click event wee need to monitor mouse up event and when it occures 
        //Two times during the time interval which is defined in Windows as a doble click time
        //we fire this event.

        /// <summary>
        /// Occurs when a double clicked was performed by the mouse. 
        /// </summary>
        public static event MouseEventHandler MouseDoubleClick
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                if (mouseDoubleClick == null)
                {
                    //We create a timer to monitor interval between two clicks
                    doubleClickTimer = new Timer
                    {
                        //This interval will be set to the value we retrieve from windows. This is a windows setting from control panel.
                        Interval = User32Library.GetDoubleClickTime(),
                        //We do not start timer yet. It will be start when the click occurs.
                        Enabled = false
                    };
                    //We define the callback function for the timer
                    doubleClickTimer.Tick += DoubleClickTimeElapsed;
                    //We start to monitor mouse up event.
                    MouseUp += OnMouseUp;
                }
                mouseDoubleClick += value;
            }
            remove
            {
                if (mouseDoubleClick != null)
                {
                    mouseDoubleClick -= value;
                    if (mouseDoubleClick == null)
                    {
                        //Stop monitoring mouse up
                        MouseUp -= OnMouseUp;
                        //Dispose the timer
                        doubleClickTimer.Tick -= DoubleClickTimeElapsed;
                        doubleClickTimer = null;
                    }
                }
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        //This field remembers mouse button pressed because in addition to the short interval it must be also the same button.
        private static MouseButtons prevClickedButton;

        //The timer to monitor time interval between two clicks.
        private static Timer doubleClickTimer;

        private static void DoubleClickTimeElapsed(object sender, EventArgs e)
        {
            //Timer is elapsed and no second click occured.
            doubleClickTimer.Enabled = false;
            prevClickedButton = MouseButtons.None;
        }

        /// <summary>
        /// This method is designed to monitor mouse clicks in order to fire a double click event if interval between 
        /// clicks was short enough.
        /// </summary>
        /// <param name="sender">Is always null</param>
        /// <param name="e">Some information about click happened.</param>
        private static void OnMouseUp(object sender, MouseEventArgs e)
        {
            //This should not happen.
            if (e.Clicks < 1)
                return;

            //If the second click happened on the same button
            if (e.Button.Equals(prevClickedButton))
            {
                //Fire double click
                mouseDoubleClick?.Invoke(null, e);
                //Stop timer
                doubleClickTimer.Enabled = false;
                prevClickedButton = MouseButtons.None;
            }
            else
            {
                //If it was the first click start the timer.
                doubleClickTimer.Enabled = true;
                prevClickedButton = e.Button;
            }
        }
        #endregion

        //################################################################
        #region Keyboard events

        private static event KeyPressEventHandler keyPress;

        /// <summary>
        /// Occurs when a key is pressed.
        /// </summary>
        /// <remarks>
        /// Key events occur in the following order: 
        /// <list type="number">
        /// <item>KeyDown</item>
        /// <item>KeyPress</item>
        /// <item>KeyUp</item>
        /// </list>
        ///The KeyPress event is not raised by noncharacter keys; however, the noncharacter keys do raise the KeyDown and KeyUp events. 
        ///Use the KeyChar property to sample keystrokes at run time and to consume or modify a subset of common keystrokes. 
        ///To handle keyboard events only in your application and not enable other applications to receive keyboard events, 
        /// set the KeyPressEventArgs.Handled property in your form's KeyPress event-handling method to <b>true</b>. 
        /// </remarks>
        public static event KeyPressEventHandler KeyPress
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                keyPress += value;
            }
            remove
            {
                keyPress -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private static event KeyEventHandler keyUp;

        /// <summary>
        /// Occurs when a key is released. 
        /// </summary>
        public static event KeyEventHandler KeyUp
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                keyUp += value;
            }
            remove
            {
                keyUp -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private static event KeyEventHandler keyDown;

        /// <summary>
        /// Occurs when a key is pressed. 
        /// </summary>
        public static event KeyEventHandler KeyDown
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                keyDown += value;
            }
            remove
            {
                keyDown -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }


        #endregion
    }
}
