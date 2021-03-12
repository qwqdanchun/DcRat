using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using Plugin;

namespace Utils
{
    class FileUtils
    {
        public static string CreateTempDuplicateFile(string filePath)
        {
            string localAppData = System.Environment.GetEnvironmentVariable("LOCALAPPDATA");
            string newFile = "";
            newFile = Path.GetRandomFileName();
            string tempFileName = localAppData + "\\Temp\\" + newFile;
            File.Copy(filePath, tempFileName);
            return tempFileName;
        }
    }

    class MiscUtils
    {
        public static void BCRYPT_INIT_AUTH_MODE_INFO(out BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO _AUTH_INFO_STRUCT_)
        {
            _AUTH_INFO_STRUCT_ = new BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO();
            _AUTH_INFO_STRUCT_.cbSize = Marshal.SizeOf(typeof(BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
            _AUTH_INFO_STRUCT_.dwInfoVersion = 1;
        }
    }
}
