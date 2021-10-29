using System.Windows.Forms;
using DustInTheWind.ActiveTime.MouseShaker.WindowsApi;

namespace DustInTheWind.ActiveTime.MouseShaker.UserActivityMonitor
{
    public class MouseEventInfo
    {
        /// <summary>
        /// Gets a signed count of the number of dents the wheel has rotated.
        /// </summary>
        public short MouseWheelDelta { get; }

        /// <summary>
        /// Gets the number of times a mouse button was pressed.
        /// </summary>
        public int ClickCount { get; }

        public bool MouseDown { get; }

        public bool MouseUp { get; }

        /// <summary>
        /// Gets a value indicating which mouse button was pressed.
        /// </summary>
        public MouseButtons Button { get; } = MouseButtons.None;

        /// <summary>
        /// Gets the x-coordinate of a mouse click, in pixels.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the y-coordinate of a mouse click, in pixels.
        /// </summary>
        public int Y { get; }

        internal MouseEventInfo(int wParam, MouseLLHookStruct mouseHookStruct)
        {
            switch (wParam)
            {
                case User32Library.WM_LBUTTONDOWN:
                    MouseDown = true;
                    Button = MouseButtons.Left;
                    ClickCount = 1;
                    break;

                case User32Library.WM_LBUTTONUP:
                    MouseUp = true;
                    Button = MouseButtons.Left;
                    ClickCount = 1;
                    break;

                case User32Library.WM_LBUTTONDBLCLK:
                    Button = MouseButtons.Left;
                    ClickCount = 2;
                    break;

                case User32Library.WM_RBUTTONDOWN:
                    MouseDown = true;
                    Button = MouseButtons.Right;
                    ClickCount = 1;
                    break;

                case User32Library.WM_RBUTTONUP:
                    MouseUp = true;
                    Button = MouseButtons.Right;
                    ClickCount = 1;
                    break;

                case User32Library.WM_RBUTTONDBLCLK:
                    Button = MouseButtons.Right;
                    ClickCount = 2;
                    break;

                case User32Library.WM_MOUSEWHEEL:
                    //If the message is WM_MOUSEWHEEL, the high-order word of MouseData member is the wheel delta. 
                    //One wheel click is defined as WHEEL_DELTA, which is 120. 
                    //(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
                    MouseWheelDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);

                    //TODO: X BUTTONS (I havent them so was unable to test)
                    //If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, 
                    //or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
                    //and the low-order word is reserved. This value can be one or more of the following values. 
                    //Otherwise, MouseData is not used. 
                    break;
            }

            X = mouseHookStruct.Point.X;
            Y = mouseHookStruct.Point.Y;
        }
    }
}