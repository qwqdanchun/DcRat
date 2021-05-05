using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace Plugin.Handler
{
    public class HandleDesktop
    {
        public void Show()
        {
            try
            {
                SetDesktopVisibility(true);
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
        }

        public void Hide()
        {
            try
            {
                SetDesktopVisibility(false);
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
        }
        public static void SetDesktopVisibility(bool visible)
        {
            var hWnd = GetDesktopWindow(DesktopWindow.ProgMan);
            Native.ShowWindow(hWnd, visible ? ShowWindowCommands.Show : ShowWindowCommands.Hide);
        }
        public enum DesktopWindow
        {
            ProgMan,
            SHELLDLL_DefViewParent,
            SHELLDLL_DefView,
            SysListView32
        }
        public static IntPtr GetDesktopWindow(DesktopWindow desktopWindow)
        {
            IntPtr progMan = Native.GetShellWindow();
            IntPtr shelldllDefViewParent = progMan;
            IntPtr shelldllDefView = Native.FindWindowEx(progMan, IntPtr.Zero, "SHELLDLL_DefView", null);
            IntPtr sysListView32 = Native.FindWindowEx(shelldllDefView, IntPtr.Zero, "SysListView32",
                "FolderView");

            if (shelldllDefView == IntPtr.Zero)
            {
                Native.EnumWindows((hwnd, lParam) =>
                {
                    const int maxChars = 256;
                    StringBuilder className = new StringBuilder(maxChars);

                    if (Native.GetClassName(hwnd, className, maxChars) > 0 && className.ToString() == "WorkerW")
                    {
                        IntPtr child = Native.FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null);
                        if (child != IntPtr.Zero)
                        {
                            shelldllDefViewParent = hwnd;
                            shelldllDefView = child;
                            sysListView32 = Native.FindWindowEx(child, IntPtr.Zero, "SysListView32", "FolderView");
                            return false;
                        }
                    }
                    return true;
                }, IntPtr.Zero);
            }

            switch (desktopWindow)
            {
                case DesktopWindow.ProgMan:
                    return progMan;
                case DesktopWindow.SHELLDLL_DefViewParent:
                    return shelldllDefViewParent;
                case DesktopWindow.SHELLDLL_DefView:
                    return shelldllDefView;
                case DesktopWindow.SysListView32:
                    return sysListView32;
                default:
                    return IntPtr.Zero;
            }
        }
    }
}