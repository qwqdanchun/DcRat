using Server.Connection;
using Server.Forms;
using Server.Helper;
using Server.MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Handle_Packet
{
    class HandleRegManager
    {
        public async void RegManager(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                switch (unpack_msgpack.ForcePathObject("Command").AsString)
                {
                    case "setClient":
                        {
                            FormRegistryEditor FM = (FormRegistryEditor)Application.OpenForms["remoteRegedit:" + unpack_msgpack.ForcePathObject("Hwid").AsString]; 
                            if (FM != null)
                            {
                                if (FM.Client == null)
                                {
                                    client.ID = unpack_msgpack.ForcePathObject("Hwid").AsString;
                                    FM.Client = client;
                                    FM.timer1.Enabled = true;
                                }
                            }
                            break;
                        }
                    case "LoadKey":
                        {
                            FormRegistryEditor FM = (FormRegistryEditor)Application.OpenForms["remoteRegedit:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
                            if (FM != null)
                            {
                                string rootKey = unpack_msgpack.ForcePathObject("RootKey").AsString;
                                byte[] Matchesbyte = unpack_msgpack.ForcePathObject("Matches").GetAsBytes();

                                BinaryFormatter formatter = new BinaryFormatter();
                                MemoryStream mStream = new MemoryStream();
                                mStream.Write(Matchesbyte, 0, Matchesbyte.Length);
                                mStream.Flush();
                                mStream.Seek(0, SeekOrigin.Begin);
                                RegistrySeeker seeker = (RegistrySeeker)formatter.Deserialize(mStream);

                                FM.AddKeys(rootKey, seeker.Matches);
                            }
                            break;
                        }

                }
            }
            catch { }
        }
    }
}
