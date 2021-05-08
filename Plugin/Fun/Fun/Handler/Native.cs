using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace Plugin.Handler
{
    public class Native
    {
        public delegate bool EnumThreadProc(IntPtr hwnd, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);


        [DllImport("user32.dll")]
        public static extern IntPtr CreateDesktop(string lpszDesktop, IntPtr lpszDevice, IntPtr pDevmode, int dwFlags, uint dwDesiredAccess, IntPtr lpsa);

        [DllImport("user32.dll")]
        public static extern bool SwitchDesktop(IntPtr hDesktop);

        [DllImport("user32.dll")]
        public static extern bool CloseDesktop(IntPtr handle);

        [DllImport("user32.dll")]
        public static extern bool SetThreadDesktop(IntPtr hDesktop);

        [DllImport("user32.dll")]
        public static extern IntPtr GetThreadDesktop(int dwThreadId);

        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr parentHwnd, IntPtr childAfterHwnd, IntPtr className,
            string windowText);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);
                
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool EnumThreadWindows(int threadId, EnumThreadProc pfnEnum, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);

        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className,
            string windowTitle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll", EntryPoint = "BlockInput")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] bool fBlockIt);

        [DllImport("user32.dll")]
        public static extern int SwapMouseButton(int bSwap);

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi)]
        public static extern int mciSendString(string lpstrCommand,
                                                  StringBuilder lpstrReturnString,
                                                  int uReturnLength,
                                                  IntPtr hwndCallback);
    }


    public enum ShowWindowCommands
    {
        Hide = 0,

        Normal = 1,

        ShowMinimized = 2,

        Maximize = 3, 

        ShowMaximized = 3,

        ShowNoActivate = 4,

        Show = 5,

        Minimize = 6,

        ShowMinNoActive = 7,

        ShowNA = 8,

        Restore = 9,

        ShowDefault = 10,

        ForceMinimize = 11
    }
}