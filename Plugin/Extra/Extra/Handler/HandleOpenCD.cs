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
    class HandleOpenCD
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi)]
        protected static extern int mciSendString(string lpstrCommand,
                                                  StringBuilder lpstrReturnString,
                                                  int uReturnLength,
                                                  IntPtr hwndCallback);
        public void Run()
        {
            mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
        }
    }
}
