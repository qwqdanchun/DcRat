using Server.Forms;
using Server.MessagePack;
using Server.Connection;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Server.Handle_Packet
{
    public class HandleFun
    {
        public void Fun(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                FormFun fun = (FormFun)Application.OpenForms["fun:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
                if (fun != null)
                {
                    if (fun.Client == null)
                    {
                        fun.Client = client;
                        fun.timer1.Enabled = true;
                    }
                }
            }
            catch { }
        }
    }
}
