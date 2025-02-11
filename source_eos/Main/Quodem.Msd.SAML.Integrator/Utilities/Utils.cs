using System;
using System.Text;

namespace Quodem.Msd.SAML.Integrator.Utilities
{
    public static class Utils
    {
        private const char CharAppend = '0';
        private const int WordLength = 32;

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string SecretGenerator(string sessionId)
        {
            sessionId = RemoveSpecialCharacters(sessionId);

            if (sessionId.Length == WordLength)
            {
                return sessionId;
            }
            if (sessionId.Length > WordLength)
            {
                return sessionId.Substring(0, WordLength);
            }
            return sessionId.PadRight(WordLength, CharAppend);
        }

        public static string ConvertFromBase64(string base64)
        {
            try
            {
                return Encoding.ASCII.GetString(Convert.FromBase64String(base64));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}