using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Logica
{
    public class Utility
    {
        
        private const int ASCII_SPACE = 32;
        private const int MAX_ASCII_PRINTABLE = 31;

        public static string ValueEncrypt(string valor)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(valor))
            {
                result = Quodem.Utility.Cryptography.SimpleEncryption.Encrypt(valor, Variables.QueryStringEncriptionKey);
            }
            return ConvertStringToHex(result);
        }

        public static string ValueDecrypt(string valor)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(valor))
            {
                valor = ConvertHexToString(valor);
                result = Quodem.Utility.Cryptography.SimpleEncryption.Decrypt(valor, Variables.QueryStringEncriptionKey);
            }
            return result;
        }

        public static string ConvertStringToHex(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }

        public static string ConvertHexToString(string HexValue)
        {
            string StrValue = "";
            while (HexValue.Length > 0)
            {
                StrValue += System.Convert.ToChar(System.Convert.ToUInt32(HexValue.Substring(0, 2), 16)).ToString();
                HexValue = HexValue.Substring(2, HexValue.Length - 2);
            }
            return StrValue;
        }

        public static string CleanString(string cadena) 
        {
            List<byte> bytes = Encoding.Unicode.GetBytes(cadena).ToList();
            List<byte> lstBytesResult = new List<byte>();
            bytes.ForEach(index =>
            {
                if (index > 0 && index <= MAX_ASCII_PRINTABLE)
                {
                    lstBytesResult.Add(ASCII_SPACE);
                }
                else
                {
                    lstBytesResult.Add(index);
                }
            });

            return Encoding.Unicode.GetString(lstBytesResult.ToArray());
        }


    }
}
