using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DustInTheWind.ActiveTime.MouseShaker.WindowsApi;

namespace DustInTheWind.ActiveTime.MouseShaker.UserActivityMonitor
{
    public static partial class HookManager
    {
        //##############################################################################
        #region Mouse hook processing

        /// <summary>
        /// This field is not objectively needed but we need to keep a reference on a delegate which will be 
        /// passed to unmanaged code. To avoid GC to clean it up.
        /// When passing delegates to unmanaged code, they must be kept alive by the managed application 
        /// until it is guaranteed that they will never be called.
        /// </summary>
        private static HookProc mouseDelegate;

        /// <summary>
        /// Stores the handle to the mouse hook procedure.
        /// </summary>
        private static int mouseHookHandle;

        private static int oldX;
        private static int oldY;

        /// <summary>
        /// A callback function which will be called every Time a mouse activity detected.
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message. 
        /// If nCode is HC_ACTION, the hook procedure must process the message. 
        /// If nCode is less than zero, the hook procedure must pass the message to the 
        /// CallNextHookEx function without further processing and must return the 
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread. 
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
        /// procedure does not call CallNextHookEx, the return value should be zero. 
        /// </returns>
        private static int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                //Marshall the data from callback.
                MouseLLHookStruct mouseHookStruct = (MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));

                //detect button clicked
                MouseButtons button = MouseButtons.None;
                short mouseDelta = 0;
                int clickCount = 0;
                bool mouseDown = false;
                bool mouseUp = false;

                switch (wParam)
                {
                    case User32Library.WM_LBUTTONDOWN:
                        mouseDown = true;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;

                    case User32Library.WM_LBUTTONUP:
                        mouseUp = true;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;

                    case User32Library.WM_LBUTTONDBLCLK:
                        button = MouseButtons.Left;
                        clickCount = 2;
                        break;

                    case User32Library.WM_RBUTTONDOWN:
                        mouseDown = true;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;

                    case User32Library.WM_RBUTTONUP:
                        mouseUp = true;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;

                    case User32Library.WM_RBUTTONDBLCLK:
                        button = MouseButtons.Right;
                        clickCount = 2;
                        break;

                    case User32Library.WM_MOUSEWHEEL:
                        //If the message is WM_MOUSEWHEEL, the high-order word of MouseData member is the wheel delta. 
                        //One wheel click is defined as WHEEL_DELTA, which is 120. 
                        //(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
                        mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);

                        //TODO: X BUTTONS (I havent them so was unable to test)
                        //If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, 
                        //or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
                        //and the low-order word is reserved. This value can be one or more of the following values. 
                        //Otherwise, MouseData is not used. 
                        break;
                }

                //generate event 
                MouseEventExtArgs e = new MouseEventExtArgs(
                                                   button,
                                                   clickCount,
                                                   mouseHookStruct.Point.X,
                                                   mouseHookStruct.Point.Y,
                                                   mouseDelta);

                //Mouse up
                if (mouseUp) 
                    HookManager.mouseUp?.Invoke(null, e);

                //Mouse down
                if (mouseDown) 
                    HookManager.mouseDown?.Invoke(null, e);

                //If someone listens to click and a click is happened
                if (clickCount > 0) 
                    mouseClick?.Invoke(null, e);

                //If someone listens to click and a click is happened
                if (clickCount > 0) 
                    mouseClickExt?.Invoke(null, e);

                //If someone listens to double click and a click is happened
                if (clickCount == 2) 
                    mouseDoubleClick?.Invoke(null, e);

                //Wheel was moved
                if (mouseDelta != 0) 
                    mouseWheel?.Invoke(null, e);

                //If someone listens to move and there was a change in coordinates raise move event
                if ((mouseMove != null || mouseMoveExt != null) && (oldX != mouseHookStruct.Point.X || oldY != mouseHookStruct.Point.Y))
                {
                    oldX = mouseHookStruct.Point.X;
                    oldY = mouseHookStruct.Point.Y;

                    mouseMove?.Invoke(null, e);
                    mouseMoveExt?.Invoke(null, e);
                }

                if (e.Handled)
                {
                    return -1;
                }
            }

            //call next hook
            return User32Library.CallNextHookEx(mouseHookHandle, nCode, wParam, lParam);
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
            //if no subscribers are registered unsubsribe from hook
            if (mouseClick == null &&
                mouseDown == null &&
                mouseMove == null &&
                mouseUp == null &&
                mouseClickExt == null &&
                mouseMoveExt == null &&
                mouseWheel == null)
            {
                ForceUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static void ForceUnsubscribeFromGlobalMouseEvents()
        {
            if (mouseHookHandle != 0)
            {
                //uninstall hook
                int result = User32Library.UnhookWindowsHookEx(mouseHookHandle);
                //reset invalid handle
                mouseHookHandle = 0;
                //Free up for GC
                mouseDelegate = null;
                //if failed and exception must be thrown
                if (result == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        #endregion

        //##############################################################################
        #region Keyboard hook processing

        /// <summary>
        /// This field is not objectively needed but we need to keep a reference on a delegate which will be 
        /// passed to unmanaged code. To avoid GC to clean it up.
        /// When passing delegates to unmanaged code, they must be kept alive by the managed application 
        /// until it is guaranteed that they will never be called.
        /// </summary>
        private static HookProc keyboardDelegate;

        /// <summary>
        /// Stores the handle to the Keyboard hook procedure.
        /// </summary>
        private static int keyboardHookHandle;

        /// <summary>
        /// A callback function which will be called every Time a keyboard activity detected.
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message. 
        /// If nCode is HC_ACTION, the hook procedure must process the message. 
        /// If nCode is less than zero, the hook procedure must pass the message to the 
        /// CallNextHookEx function without further processing and must return the 
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread. 
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
        /// procedure does not call CallNextHookEx, the return value should be zero. 
        /// </returns>
        private static int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            //indicates if any of underlying events set e.Handled flag
            bool handled = false;

            if (nCode >= 0)
            {
                //read structure KeyboardHookStruct at lParam
                KeyboardHookStruct keyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                //raise KeyDown
                if (keyDown != null && (wParam == User32Library.WM_KEYDOWN || wParam == User32Library.WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)keyboardHookStruct.VirtualKeyCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    keyDown.Invoke(null, e);
                    handled = e.Handled;
                }

                // raise KeyPress
                if (keyPress != null && wParam == User32Library.WM_KEYDOWN)
                {
                    bool isDownShift = (User32Library.GetKeyState(User32Library.VK_SHIFT) & 0x80) == 0x80;
                    bool isDownCapslock = User32Library.GetKeyState(User32Library.VK_CAPITAL) != 0;

                    byte[] keyState = new byte[256];
                    User32Library.GetKeyboardState(keyState);
                    byte[] inBuffer = new byte[2];
                    if (User32Library.ToAscii(keyboardHookStruct.VirtualKeyCode,
                            keyboardHookStruct.ScanCode,
                            keyState,
                            inBuffer,
                            keyboardHookStruct.Flags) == 1)
                    {
                        char key = (char)inBuffer[0];
                        if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
                        KeyPressEventArgs e = new KeyPressEventArgs(key);
                        keyPress.Invoke(null, e);
                        handled = handled || e.Handled;
                    }
                }

                // raise KeyUp
                if (keyUp != null && (wParam == User32Library.WM_KEYUP || wParam == User32Library.WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)keyboardHookStruct.VirtualKeyCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    keyUp.Invoke(null, e);
                    handled = handled || e.Handled;
                }

            }

            //if event handled in application do not handoff to other listeners
            if (handled)
                return -1;

            //forward to other application
            return User32Library.CallNextHookEx(keyboardHookHandle, nCode, wParam, lParam);
        }

        private static void EnsureSubscribedToGlobalKeyboardEvents()
        {
            // install Keyboard hook only if it is not installed and must be installed
            if (keyboardHookHandle == 0)
            {
                //See comment of this field. To avoid GC to clean it up.
                keyboardDelegate = KeyboardHookProc;
                //install hook
                keyboardHookHandle = User32Library.SetWindowsHookEx(
                    User32Library.WH_KEYBOARD_LL,
                    keyboardDelegate,
                    Marshal.GetHINSTANCE(
                        Assembly.GetExecutingAssembly().GetModules()[0]),
                    0);
                //If SetWindowsHookEx fails.
                if (keyboardHookHandle == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //do cleanup

                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        private static void TryUnsubscribeFromGlobalKeyboardEvents()
        {
            //if no subscribers are registered unsubscribe from hook
            if (keyDown == null &&
                keyUp == null &&
                keyPress == null)
            {
                ForceUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private static void ForceUnsubscribeFromGlobalKeyboardEvents()
        {
            if (keyboardHookHandle != 0)
            {
                //uninstall hook
                int result = User32Library.UnhookWindowsHookEx(keyboardHookHandle);
                //reset invalid handle
                keyboardHookHandle = 0;
                //Free up for GC
                keyboardDelegate = null;
                //if failed and exception must be thrown
                if (result == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        #endregion

    }
}
