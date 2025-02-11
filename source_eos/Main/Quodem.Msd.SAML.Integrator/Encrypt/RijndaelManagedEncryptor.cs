using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Quodem.Msd.SAML.Integrator.Encrypt
{
    public class RijndaelManagedEncryptor
    {
        #region Consts

        /// <summary>
        /// Change the Inputkey GUID when you use this code in your own program.
        /// Keep this inputkey very safe and prevent someone from decoding it some way!!
        /// </summary>
        /// <summary>
        /// Encrypt the given text and give the byte array back as a BASE64 string
        /// </summary>
        /// <param name="text">The text to encrypt</param>
        /// <param name="inputkey">the password key</param>
        /// <param name="salt">The pasword salt</param>
        /// <returns>The encrypted text</returns>
        public static string EncryptRijndael(string text, string inputkey, string salt)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");

            var aesAlg = NewRijndaelManaged(inputkey, salt);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }
        #endregion

        #region Rijndael Dycryption
        /// <summary>
        /// Checks if a string is base64 encoded
        /// </summary>
        /// <param name="base64String">The base64 encoded string</param>
        /// <returns></returns>
        public static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            return (base64String.Length % 4 == 0) &&
                   Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }

        /// <summary>
        /// Decrypts the given text
        /// </summary>
        /// <param name="cipherText">The encrypted BASE64 text</param>
        /// <param name="inputkey">the password key</param>
        /// <param name="salt">The pasword salt</param>
        /// <returns>De gedecrypte text</returns>
        public static string DecryptRijndael(string cipherText, string inputkey, string salt)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");

            if (!IsBase64String(cipherText))
                throw new Exception("The cipherText input parameter is not base64 encoded");

            string text;

            var aesAlg = NewRijndaelManaged(inputkey, salt);
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var cipher = Convert.FromBase64String(cipherText);

            using (var msDecrypt = new MemoryStream(cipher))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        text = srDecrypt.ReadToEnd();
                    }
                }
            }
            return text;
        }
        #endregion

        #region NewRijndaelManaged

        /// <summary>
        /// Create a new RijndaelManaged class and initialize it
        /// </summary>
        /// <param name="inputkey">the password key</param>
        /// <param name="salt">The pasword salt</param>
        /// <returns></returns>
        private static RijndaelManaged NewRijndaelManaged(string inputkey, string salt)
        {
            if (salt == null) throw new ArgumentNullException("salt");
            if (inputkey == null) throw new ArgumentNullException("inputkey");
            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var key = new Rfc2898DeriveBytes(inputkey, saltBytes);

            var aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

            return aesAlg;
        }
        #endregion
    }
}
