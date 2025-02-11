using System;
using System.Security.Cryptography;
using System.Text;

namespace Quodem.Msd.SAML.Integrator.Utilities
{
    public static class Sha1Generator
    {
        public static string GetHash(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        public static string Minus3_GetHash(string s)
        {
            string r = GetHash(s);

            try {

                r = r.Substring( 3, r.Length-3 );

            } catch (Exception ex)
            {
                r = "";
            }

            return r;
        }

        private static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}