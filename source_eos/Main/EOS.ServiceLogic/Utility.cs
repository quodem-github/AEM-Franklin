using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using EOS.ServiceLogic.BLL.Token;
using EOS.ServiceModel;
using NHibernate;
using NHibernate.Linq;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace EOS.ServiceLogic
{
    public class Utility
    {
        /// <summary>
        /// TODO: Separar responsabilidades una cosas es encriptacion y otra el servicio de encriptación 
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string Decrypt3DES(QSuscriptor suscriptor, string encryptedData, string token)
        {
            var cipher = global::Org.BouncyCastle.Security.CipherUtilities.GetCipher(Variables.ENCRYPTION_ALGORITHM_NAME);

            byte[] byteKey = Encoding.UTF8.GetBytes(suscriptor.Privatekey);
            var param_key = new DesEdeParameters(byteKey);

            cipher.Init(false, param_key);

            byte[] secretBytes = Convert.FromBase64String(encryptedData);
            byte[] resultArray = cipher.DoFinal(secretBytes);

            var result = Encoding.UTF8.GetString(resultArray);

            return result;
        }

        public static string Encrypt3DES(QSuscriptor suscriptor, string rawData, string token)
        {
            var cipher = CipherUtilities.GetCipher("DESEDE");
            byte[] byte_key = Encoding.UTF8.GetBytes(suscriptor.Privatekey);
            var param_key = new DesEdeParameters(byte_key);
            byte[] data = Encoding.UTF8.GetBytes(rawData);
            cipher.Init(true, param_key);
            var data_encrypted = cipher.DoFinal(data);
            string result = Convert.ToBase64String(data_encrypted);

            return result;
        }

        public static string GetJsonStr(object obj)
        {
            string json;
            using (var ms = new MemoryStream())
            {
                var ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
                ser.WriteObject(ms, obj);
                json = System.Text.Encoding.UTF8.GetString(ms.GetBuffer(), 0, Convert.ToInt32(ms.Length));
            }
            return json;
        }

        /// <summary>
        /// Returns enum display name
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetDisplayName(Type enumType, string name)
        {
            try
            {
                var result = name;

                var attribute =
                    enumType.GetField(name)
                        .GetCustomAttributes(inherit: false)
                        .OfType<DisplayAttribute>()
                        .FirstOrDefault();

                if (attribute != null)
                {
                    result = attribute.GetName();
                }

                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
