using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_GENERAL.Helper
{
    public static class FileUtils
    {
        public static string ConvertToBase64(string physicalPath)
        {
            Byte[] bytes = File.ReadAllBytes(physicalPath);
            return Convert.ToBase64String(bytes);
        }
        public static bool CheckFilePath(string physicalPath)
        {
            return File.Exists(physicalPath);
        }
    }
}
