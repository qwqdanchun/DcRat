using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace Plugin.Handler
{
    public class HandleTaskbar
    {
        private const string VistaStartMenuCaption = "Start";
        private static IntPtr vistaStartMenuWnd = IntPtr.Zero;


        public void Show()
        {
            try
            {
                SetVisibility(true);
            }
            catch(Exception ex)
            {
                Packet.Error(ex.Message);
            }            
        }

        public void Hide()
        {
            try
            {
                SetVisibility(false);
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
        }

        private static void SetVisibility(bool show)
        {
            IntPtr taskBarWnd = Native.FindWindow("Shell_TrayWnd", null);

            IntPtr startWnd = Native.FindWindowEx(IntPtr.Zero, IntPtr.Zero, (IntPtr)0xC017, "Start");


            if (startWnd == IntPtr.Zero)
            {
                startWnd = Native.FindWindow("Button", null);

                if (startWnd == IntPtr.Zero)
                {
                    startWnd = GetVistaStartMenuWnd(taskBarWnd);
                }
            }

            Native.ShowWindow(taskBarWnd, show ? ShowWindowCommands.Show : ShowWindowCommands.Hide);
            Native.ShowWindow(startWnd, show ? ShowWindowCommands.Show : ShowWindowCommands.Hide);
        }

        private static IntPtr GetVistaStartMenuWnd(IntPtr taskBarWnd)
        {
            uint procId;
            Native.GetWindowThreadProcessId(taskBarWnd, out procId);

            Process p = Process.GetProcessById((int)procId);

            foreach (ProcessThread t in p.Threads)
            {
                Native.EnumThreadWindows(t.Id, MyEnumThreadWindowsProc, IntPtr.Zero);
            }

            return vistaStartMenuWnd;
        }
        private static bool MyEnumThreadWindowsProc(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder buffer = new StringBuilder(256);
            if (Native.GetWindowText(hWnd, buffer, buffer.Capacity) > 0)
            {
                if (buffer.ToString() == VistaStartMenuCaption)
                {
                    vistaStartMenuWnd = hWnd;
                    return false;
                }
            }
            return true;
        }
    }


}