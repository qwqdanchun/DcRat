using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace Plugin.Handler
{
    class HandleMonitor
    {
        // ReSharper disable InconsistentNaming
        private const int HWND_BROADCAST = 0xFFFF;
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MONITORPOWER = 0xF170;
        // ReSharper restore InconsistentNaming

        public void TurnOff()
        {
            Native.SendMessage(new IntPtr(HWND_BROADCAST), WM_SYSCOMMAND, new IntPtr(SC_MONITORPOWER),
                new IntPtr(2));
        }
    }
}
