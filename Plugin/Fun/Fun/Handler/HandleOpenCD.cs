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
        public void Show()
        {
            Native.mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
        }

        public void Hide()
        {
            Native.mciSendString("set CDAudio door closed", null, 0, IntPtr.Zero);
        }
    }
}
