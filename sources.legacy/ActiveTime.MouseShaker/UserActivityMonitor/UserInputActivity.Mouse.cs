using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DustInTheWind.ActiveTime.MouseShaker.WindowsApi;

namespace DustInTheWind.ActiveTime.MouseShaker.UserActivityMonitor
{
    public static partial class UserInputActivity
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
        //To fire the double click event wee need to monitor mouse up event and when it occurs 
        //Two times during the time interval which is defined in Windows as a double click time
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

        //##############################################################################

        #region Mouse hook processing

        /// <summary>
        /// This field is not objectively needed but we need to keep a reference on a delegate which will be 
        /// passed to unmanaged code. To avoid GC to clean it up.
        /// When passing delegates to unmanaged code, they must be kept alive by the managed application 
        /// until it is guaranteed that they will never be called.
        /// </summary>
        private static HookProc mouseDelegate;

        private static int mouseHookHandle;

        private static int oldX;
        private static int oldY;

        private static int MouseHookProc(int code, int wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                //Marshall the data from callback.
                MouseLLHookStruct mouseHookStruct = (MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));

                MouseEventInfo mouseEventInfo = new MouseEventInfo(wParam, mouseHookStruct);
                MouseEventExtArgs args = new MouseEventExtArgs(mouseEventInfo);

                if (mouseEventInfo.MouseUp)
                    mouseUp?.Invoke(null, args);

                if (mouseEventInfo.MouseDown)
                    mouseDown?.Invoke(null, args);

                if (mouseEventInfo.ClickCount > 0)
                    mouseClick?.Invoke(null, args);

                if (mouseEventInfo.ClickCount > 0)
                    mouseClickExt?.Invoke(null, args);

                if (mouseEventInfo.ClickCount == 2)
                    mouseDoubleClick?.Invoke(null, args);

                if (mouseEventInfo.MouseWheelDelta != 0)
                    mouseWheel?.Invoke(null, args);

                //If someone listens to move and there was a change in coordinates raise move event
                if (oldX != mouseEventInfo.X || oldY != mouseEventInfo.Y)
                {
                    oldX = mouseEventInfo.X;
                    oldY = mouseEventInfo.Y;

                    mouseMove?.Invoke(null, args);
                    mouseMoveExt?.Invoke(null, args);
                }

                if (args.Handled)
                    return -1;
            }

            return User32Library.CallNextHookEx(mouseHookHandle, code, wParam, lParam);
        }

        private static void EnsureSubscribedToGlobalMouseEvents()
        {
            // install Mouse hook only if it is not installed and must be installed
            if (mouseHookHandle == 0)
            {
                //See comment of this field. To avoid GC to clean it up.
                mouseDelegate = MouseHookProc;
                //install hook
                //mouseHookHandle = SetWindowsHookEx(
                //    WH_MOUSE_LL,
                //    mouseDelegate,
                //    Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
                //    0);
                mouseHookHandle = User32Library.SetWindowsHookEx(User32Library.WH_MOUSE_LL, mouseDelegate, IntPtr.Zero, 0);
                //If SetWindowsHookEx fails.
                if (mouseHookHandle == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //do cleanup

                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        private static void TryUnsubscribeFromGlobalMouseEvents()
        {
            bool noSubscribeIsRegistered = mouseClick == null && mouseDown == null && mouseMove == null &&
                                           mouseUp == null && mouseClickExt == null && mouseMoveExt == null &&
                                           mouseWheel == null;

            if (noSubscribeIsRegistered)
                ForceUnsubscribeFromGlobalMouseEvents();
        }

        private static void ForceUnsubscribeFromGlobalMouseEvents()
        {
            if (mouseHookHandle == 0)
                return;

            int result = User32Library.UnhookWindowsHookEx(mouseHookHandle);

            mouseHookHandle = 0;
            mouseDelegate = null;

            if (result == 0)
            {
                // Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                int errorCode = Marshal.GetLastWin32Error();

                // Initializes and throws a new instance of the Win32Exception class with the specified error. 
                throw new Win32Exception(errorCode);
            }
        }

        #endregion
    }
}