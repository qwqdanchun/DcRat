using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace Plugin.Handler
{
    public class HandleClock
    {
        public void Show()
        {
            try
            {
                SetClockVisibility(true);
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
                SetClockVisibility(false);
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
        }
        public static void SetClockVisibility(bool visible)
        {
            var hWnd = Native.GetDlgItem(Native.FindWindow("Shell_TrayWnd", null), 0x12F);
            hWnd = Native.GetDlgItem(hWnd, 0x12F);
            Native.ShowWindow(hWnd, visible ? ShowWindowCommands.Show : ShowWindowCommands.Hide);
        }
    }


}