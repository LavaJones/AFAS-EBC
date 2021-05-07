using System;
using System.IO;
using System.Security.Cryptography;
using log4net;

namespace Afas.AfComply.Domain
{
    public static class AesEncryption
    {
        private static ILog Log = LogManager.GetLogger(typeof(AesEncryption));

        private static string key = "1234567890_d1234567890_d123456kf";
        private static string IV = "asfdasdfasdf_45#";

        private static AesCryptoServiceProvider singleton = null;

        private static AesCryptoServiceProvider aes
        {
            get
            {

                if (singleton == null)
                {
                    singleton = new AesCryptoServiceProvider();
                    singleton.BlockSize = 128;
                    singleton.KeySize = 256;
                    singleton.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
                    singleton.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV);
                    singleton.Padding = PaddingMode.PKCS7;
                    singleton.Mode = CipherMode.CBC;

                }

                return singleton;
            }
        }

        public static string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                return string.Empty;
            }

            if (encryptedText.ZeroPadSsn().IsValidSsn() || encryptedText.Contains("****"))
            {
                return encryptedText;
            }

            try
            {
                byte[] encryptedbytes = Convert.FromBase64String(encryptedText);

                ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] plainText = crypto.TransformFinalBlock(encryptedbytes, 0, encryptedbytes.Length);
                crypto.Dispose();

                return System.Text.ASCIIEncoding.ASCII.GetString(plainText);
            }
            catch (Exception ex)
            {
                Log.Warn("Error during decryption.",ex);

                return encryptedText;
            }
        }

        public static string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (false == text.ZeroPadSsn().IsValidSsn() && false == text.Contains("****"))
            {
                throw new ArgumentException("Tried to encrypt SSN that was not valid.");
            }

            try
            {
                byte[] plaintextbytes = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
                ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] encrypted = crypto.TransformFinalBlock(plaintextbytes, 0, plaintextbytes.Length);
                crypto.Dispose();
                return Convert.ToBase64String(encrypted);
            }
            catch (Exception ex)
            {
                Log.Warn("Error during encryption.",ex);

                return text;
            }
        }

    }
}