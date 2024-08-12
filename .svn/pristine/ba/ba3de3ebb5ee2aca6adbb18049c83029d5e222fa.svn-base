using System;
using System.Runtime.InteropServices;

namespace NCManagementSystem.Libraries.Controls
{
    public class NativeMethods
    {
        public static int WS_EX_COMPOSITED = 0x02000000;
        public static int WS_EX_TRANSPARENT = 0x00000020;

        public const int WM_NCLBUTTONDOWN = 0x0A1;
        public const int HTCAPTION = 0x2;

        public const int HTLEFT = 10;
        public const int HTRIGHT = 11;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 0x10;
        public const int HTBOTTOMRIGHT = 17;

        public const int WM_SETFOCUS = 7;
        public const int WM_KILLFOCUS = 8;
        public const int WM_PAINT = 15;

        public const int DTM_First = 0x1000;
        public const int DTM_GETMONTHCAL = (DTM_First + 8);
        public const int MCM_GETMINREQRECT = (DTM_First + 9);
        public const int SWP_NOMOVE = 0x0002;
        public const int WM_SYSKEYDOWN = 0x104;

        public const int WM_LBUTTONDOWN = 0x201;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECTANGLE { public int Left, Top, Right, Bottom; }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, ref RECTANGLE lParam);

        [DllImport("uxtheme.dll")]
        public static extern int SetWindowTheme(IntPtr hWnd, string appName, string idList);

        [DllImport("User32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);


        public static int WM_SETREDRAW = 0x000B;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
    }
}
