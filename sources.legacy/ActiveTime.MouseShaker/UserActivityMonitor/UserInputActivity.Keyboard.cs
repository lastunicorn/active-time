using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DustInTheWind.ActiveTime.MouseShaker.WindowsApi;

namespace DustInTheWind.ActiveTime.MouseShaker.UserActivityMonitor
{

    /// <summary>
    /// This class monitors all mouse activities globally (also outside of the application) 
    /// and provides appropriate events.
    /// </summary>
    public static partial class UserInputActivity
    {
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

        //##############################################################################

        #region Keyboard hook processing

        /// <summary>
        /// This field is not objectively needed but we need to keep a reference on a delegate which will be 
        /// passed to unmanaged code. To avoid GC to clean it up.
        /// When passing delegates to unmanaged code, they must be kept alive by the managed application 
        /// until it is guaranteed that they will never be called.
        /// </summary>
        private static HookProc keyboardDelegate;

        private static int keyboardHookHandle;

        private static int KeyboardHookProc(int code, int wParam, IntPtr lParam)
        {
            //indicates if any of underlying events set e.Handled flag
            bool handled = false;

            if (code >= 0)
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
                        if ((isDownCapslock ^ isDownShift) && char.IsLetter(key)) key = char.ToUpper(key);
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

            return handled
                ? -1
                : User32Library.CallNextHookEx(keyboardHookHandle, code, wParam, lParam);
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
                    Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
                    0);

                //If SetWindowsHookEx fails.
                if (keyboardHookHandle == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();

                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        private static void TryUnsubscribeFromGlobalKeyboardEvents()
        {
            bool noSubscribersAreRegistered = keyDown == null && keyUp == null && keyPress == null;

            if (noSubscribersAreRegistered)
                ForceUnsubscribeFromGlobalKeyboardEvents();
        }

        private static void ForceUnsubscribeFromGlobalKeyboardEvents()
        {
            if (keyboardHookHandle == 0)
                return;

            int result = User32Library.UnhookWindowsHookEx(keyboardHookHandle);

            keyboardHookHandle = 0;
            keyboardDelegate = null;

            if (result == 0)
            {
                //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                int errorCode = Marshal.GetLastWin32Error();
                //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                throw new Win32Exception(errorCode);
            }
        }

        #endregion
    }
}
