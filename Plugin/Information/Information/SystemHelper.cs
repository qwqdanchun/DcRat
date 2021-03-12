using System;
using System.Management;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Plugin
{
    public static class SystemHelper
    {
        public static string GetUptime()
        {
            try
            {
                string uptime = string.Empty;

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem WHERE Primary='true'"))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        DateTime lastBootUpTime = ManagementDateTimeConverter.ToDateTime(mObject["LastBootUpTime"].ToString());
                        TimeSpan uptimeSpan = TimeSpan.FromTicks((DateTime.Now - lastBootUpTime).Ticks);

                        uptime = string.Format("{0}d : {1}h : {2}m : {3}s", uptimeSpan.Days, uptimeSpan.Hours, uptimeSpan.Minutes, uptimeSpan.Seconds);
                        break;
                    }
                }

                if (string.IsNullOrEmpty(uptime))
                    throw new Exception("Getting uptime failed");

                return uptime;
            }
            catch (Exception)
            {
                return string.Format("{0}d : {1}h : {2}m : {3}s", 0, 0, 0, 0);
            }
        }

        public static string GetAntivirus()
        {
            MessageBox.Show("98");
            try
            {
                string antivirusName = string.Empty;
                // starting with Windows Vista we must use the root\SecurityCenter2 namespace
                string scope = (PlatformHelper.VistaOrHigher) ? "root\\SecurityCenter2" : "root\\SecurityCenter";
                string query = "SELECT * FROM AntivirusProduct";

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        antivirusName += mObject["displayName"].ToString() + "; ";
                    }
                }
                antivirusName = StringHelper.RemoveLastChars(antivirusName);

                return (!string.IsNullOrEmpty(antivirusName)) ? antivirusName : "N/A";
            }
            catch
            {
                return "Unknown";
            }
        }

        public static string GetFirewall()
        {
            try
            {
                string firewallName = string.Empty;
                // starting with Windows Vista we must use the root\SecurityCenter2 namespace
                string scope = (PlatformHelper.VistaOrHigher) ? "root\\SecurityCenter2" : "root\\SecurityCenter";
                string query = "SELECT * FROM FirewallProduct";

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        firewallName += mObject["displayName"].ToString() + "; ";
                    }
                }
                firewallName = StringHelper.RemoveLastChars(firewallName);

                return (!string.IsNullOrEmpty(firewallName)) ? firewallName : "N/A";
            }
            catch
            {
                return "Unknown";
            }
        }
        public static class PlatformHelper
        {
            
            public static bool VistaOrHigher { get; }

            
        }
        public static class StringHelper
        {
            /// <summary>
            /// Gets the formatted MAC address.
            /// </summary>
            /// <param name="macAddress">The unformatted MAC address.</param>
            /// <returns>The formatted MAC address.</returns>
            public static string GetFormattedMacAddress(string macAddress)
            {
                return (macAddress.Length != 12)
                    ? "00:00:00:00:00:00"
                    : Regex.Replace(macAddress, "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})", "$1:$2:$3:$4:$5:$6");
            }

            /// <summary>
            /// Safely removes the last N chars from a string.
            /// </summary>
            /// <param name="input">The input string.</param>
            /// <param name="amount">The amount of last chars to remove (=N).</param>
            /// <returns>The input string with N removed chars.</returns>
            public static string RemoveLastChars(string input, int amount = 2)
            {
                if (input.Length > amount)
                    input = input.Remove(input.Length - amount);
                return input;
            }

            public class SafeRandom
            {
                private static readonly RandomNumberGenerator GlobalCryptoProvider = RandomNumberGenerator.Create();

                [ThreadStatic]
                private static Random _random;

                private static Random GetRandom()
                {
                    if (_random == null)
                    {
                        byte[] buffer = new byte[4];
                        GlobalCryptoProvider.GetBytes(buffer);
                        _random = new Random(BitConverter.ToInt32(buffer, 0));
                    }

                    return _random;
                }
            }
        }
    }
}
