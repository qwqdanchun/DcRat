using System;
using System.Runtime.InteropServices;

namespace Server.Helper.Donut.Structs
{
    public struct DSFileInfo
    {
        public int fd;
        public UInt64 size;
        public byte map;
        public int type;
        public int arch;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.DONUT_VER_LEN)]
        public char[] ver;
    }
}
