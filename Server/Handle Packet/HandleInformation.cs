using Server.Forms;
using Server.MessagePack;
using Server.Connection;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using Server.Helper;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Server.Handle_Packet
{
    public class HandleInformation
    {
        public void AddToInformationList(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                string tempPath = Path.Combine(Application.StartupPath, "ClientsFolder\\" + unpack_msgpack.ForcePathObject("ID").AsString + "\\Information");
                string fullPath = tempPath + $"\\Information.txt";
                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);
                File.WriteAllText(fullPath, unpack_msgpack.ForcePathObject("InforMation").AsString);
                Process.Start("explorer.exe", fullPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
