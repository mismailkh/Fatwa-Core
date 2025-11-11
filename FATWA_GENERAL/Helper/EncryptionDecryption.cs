using System.Security.Cryptography;
using System.Text;

namespace FATWA_GENERAL.Helper
{
    public class EncryptionDecryption
    {
        public static string EncryptText(string text, string password)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] inputByteArray = UE.GetBytes(text);
            byte[] key = UE.GetBytes(password);
            RijndaelManaged RMCrypto = new RijndaelManaged();
            var ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string DecryptText(string text, string password)
        {
            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] inputByteArray = Convert.FromBase64String(text);
                byte[] key = UE.GetBytes(password);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                var ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return UE.GetString(ms.ToArray());
            } 
            catch(Exception ex)
            {
                return "";
            }
        }
    }
}
