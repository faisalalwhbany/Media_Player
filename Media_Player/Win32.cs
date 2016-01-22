using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Win32
{
    class Api
    {
        public const uint WS_EX_STATICEDGE = 0x00020000;
        public const uint WS_CHILD = 0x40000000;
        public const uint WS_VISIBLE = 0x10000000;
        public const uint WS_HSCROLL = 0x00100000;
        public const uint WS_VSCROLL = 0x00200000;

        public const int GWL_WNDPROC = -4;

        public const int WM_ACTIVATEAPP = 0x1C;
        public const int WM_COMMAND = 0x111;
        public const int WM_CLOSE = 0x10;
        public const int WM_DESTROY = 2;
        public const int WM_DROPFILES = 0x233;
        public const int WM_ERASEBKGND = 0x14;
        public const int WM_KEYDOWN = 256;
        public const int WM_LBUTTONDBLCLK = 515;
        public const int WM_LBUTTONDOWN = 513;
        public const int WM_LBUTTONUP = 514;
        public const int WM_MOVE = 0x3;
        public const int WM_MOUSEMOVE = 512;
        public const int WM_MOVING = 0x216;
        public const int WM_NCHITTEST = 0x0084;
        public const int WM_NCLBUTTONDOWN = 161;
        public const int WM_NCLBUTTONDBLCLK = 163;
        public const int WM_PAINT = 0xF;
        public const int WM_RBUTTONDOWN = 516;
        public const int WM_TIMER = 0x113;
        public const int WM_PRINT = 0x317;
        public const int WM_PRINTCLIENT = 0x318;
        public const int WM_SETREDRAW = 0xB;
        public const int WM_SIZING = 0x214;
        public const int WM_SIZE = 0x5;
        public const int WM_USER = 0x400;
        public const int WM_STRINGDATA = WM_USER + 3;

        public const int VK_HOME = 36;
        public const int VK_END = 35;
        public const int VK_PRIOR = 33;
        public const int VK_NEXT = 34;
        public const int VK_LEFT = 37;
        public const int VK_UP = 38;
        public const int VK_RIGHT = 39;
        public const int VK_DOWN = 40;

        public const int VK_NUMPAD1 = 97;
        public const int VK_NUMPAD2 = 98;
        public const int VK_NUMPAD3 = 99;
        public const int VK_NUMPAD4 = 100;
        public const int VK_NUMPAD5 = 101;
        public const int VK_NUMPAD6 = 102;
        public const int VK_NUMPAD7 = 103;
        public const int VK_NUMPAD8 = 104;
        public const int VK_NUMPAD9 = 105;

        public const int VK_PGUP = 0x21;
        public const int VK_PGDN = 0x22;

        public const int GWL_ID = -12;

        public const int PRF_CLIENT = 0x00000004;
        public const int PRF_CHILDREN = 0x00000010;

        public const int HTNOWHERE = 0;
        public const int HTCLIENT = 1;
        public const int HTCAPTION = 2;
        public const int HTSYSMENU = 3;
        public const int HTGROWBOX = 4;
        public const int HTMENU = 5;
        public const int HTHSCROLL = 6;
        public const int HTVSCROLL = 7;
        public const int HTMINBUTTON = 8;
        public const int HTMAXBUTTON = 9;
        public const int HTLEFT = 10;
        public const int HTRIGHT = 11;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 16;
        public const int HTBOTTOMRIGHT = 17;
        public const int HTBORDER = 18;
        public const int HTOBJECT = 19;
        public const int HTCLOSE = 20;
        public const int HTHELP = 21;
        public const int MAX_PATH = 260;
        public const int SRCCOPY = 0x00CC0020;
        public const int GA_PARENT = 1;
        public const int GA_ROOT = 2;
        public const int GA_ROOTOWNER = 3;

        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOREDRAW = 0x0008;
        public const uint SWP_NOACTIVATE = 0x0010;
        public const uint SWP_NOOWNERZORDER = 0x0200;  // Don't do owner Z ordering
        public const uint SWP_NOSENDCHANGING = 0x0400; // Don't send WM_WINDOWPOSCHANGING

        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;
        public const int SW_RESTORE = 9;

        public const int RGN_AND = 1;
        public const int RGN_OR = 2;
        public const int RGN_XOR = 3;
        public const int RGN_DIFF = 4;
        public const int RGN_COPY = 5;
        public const int RGN_MIN = RGN_AND;
        public const int RGN_MAX = RGN_COPY;

        public const int FW_BOLD = 700;

        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;

        private const string USER32 = "User32.DLL";
        private const string GDI32 = "GDI32.DLL";
        private const string KERNEL32 = "kernel32.Dll";
        private const string SHELL32 = "SHELL32.DLL";

        

        [StructLayout(LayoutKind.Explicit)]
        public struct RECT
        {
            [FieldOffset(0)]
            public int left;
            [FieldOffset(4)]
            public int top;
            [FieldOffset(8)]
            public int right;
            [FieldOffset(12)]
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

       

        [DllImport(USER32)] // Win32 encapsulation
        public static extern bool PtInRect(ref RECT r, POINT p);


        [DllImport(USER32, EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hWnd, int dwMsg, uint wParam, int lParam);

        

        [DllImport(USER32)]
        public static extern IntPtr GetForegroundWindow();

        

    }
}
