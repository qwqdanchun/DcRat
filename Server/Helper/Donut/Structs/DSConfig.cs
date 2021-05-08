using System;
using System.Runtime.InteropServices;

namespace Server.Helper.Donut.Structs
{
    public unsafe struct DSConfig
    {
        public int arch;
        public int bypass;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.DONUT_MAX_NAME)]
        public char[] domain;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.DONUT_MAX_NAME)]
        public char[] cls;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.DONUT_MAX_NAME)]
        public char[] method;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (Constants.DONUT_MAX_PARAM+1)* Constants.DONUT_MAX_NAME)]
        public char[] param;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.DONUT_MAX_NAME)]
        public char[] file;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.DONUT_MAX_URL)]
        public char[] url;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.DONUT_MAX_NAME)]
        public char[] runtime;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.DONUT_MAX_NAME)]
        public char[] modname;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.DONUT_MAX_NAME)]
        public char[] outfile;

        public int mod_type;
        public UInt64 mod_len;
        public DSModule mod;

        public int inst_type;
        public UInt64 inst_len;
        public DSInstance inst;

        public int pic_cnt;
        public UInt64 pic_len;
        public IntPtr pic;
    }
}
