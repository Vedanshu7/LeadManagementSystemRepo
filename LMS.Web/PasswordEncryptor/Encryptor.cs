using LMS.Common.Encryption;
using System.Configuration;
using System.Security.Cryptography;

namespace LMS.Web.PasswordEncryptor
{
    public class Encryptor
    {
        public static string Encryption(string text)
        {
            string ciphertext;
            using (AesManaged myAes = new AesManaged())
            {
                string key = ConfigurationManager.AppSettings.Get("key");
                ciphertext = AESAlgorithm.Encrypt(text, key);
            }
            return ciphertext;
        }
        public static string Decryption(string text)
        {
            string normaltext;
            using (AesManaged myAes = new AesManaged())
            {
                string key = ConfigurationManager.AppSettings.Get("key");
                normaltext = AESAlgorithm.Decrypt(text, key);
            }
            return normaltext;
        }
    }
}