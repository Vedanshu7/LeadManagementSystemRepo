using LMS.Common.Encryption;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace LMS.Web.PasswordEncryptor
{
    public class Encryptor
    {
        public static string Encryption(string text)
        {
            string  ciphertext;
            using (AesManaged myAes = new AesManaged())
            {
                string key = ConfigurationManager.AppSettings.Get("key");
                ciphertext = AESAlgorithm.Encrypt(text, key);
            }
            return ciphertext;
        }
    }
}