using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace Plugin.Handler
{
    public class HandleMouseButton
    {
        public void RestoreMouseButtons()
        {
            try
            {
                Native.SwapMouseButton(0);
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
        }

        public void SwapMouseButtons()
        {
            try
            {
                Native.SwapMouseButton(1);
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
        }
    }


}