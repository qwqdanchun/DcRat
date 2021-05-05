using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace Plugin.Handler
{
    class HandlePlayAudio
    {
        public void Play(byte[] wavfile)
        {
            string fullPath = Path.Combine(Path.GetTempPath(), GetRandomString(6) + ".wav");

            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                fs.Write(wavfile, 0, wavfile.Length);


            SoundPlayer sp = new SoundPlayer(fullPath);
            sp.Load();
            sp.Play();
        }
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
        public static Random Random = new Random();
        public static string GetRandomString(int length)
        {
            StringBuilder randomName = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                randomName.Append(Alphabet[Random.Next(Alphabet.Length)]);

            return randomName.ToString();
        }
    }
}
