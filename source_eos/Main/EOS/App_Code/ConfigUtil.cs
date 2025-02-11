using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.IO;

namespace EOS
{
    /// <summary>
    /// Utility class wrapper to read config settings from the web.config file
    /// You can use machinenames in the config settings adding the same key various times
    /// Allowing you to set the keys for different machines in 1 config
    /// Also, you can use encrypted keys, which are automatically decrypted by this class.
    /// 
    /// Written by e.bloem, Qurius: 07-02-2011
    /// </summary>
    public class ConfigUtil
    {
        private static string machineName;

        static string GetMachineName()
        {
            if (ConfigUtil.machineName != null)
                return ConfigUtil.machineName;
            ConfigUtil.machineName = Environment.MachineName;
            return ConfigUtil.machineName;
        }

        private static string ReadConnectionString(string key)
        {
            string machineKey = GetMachineName() + ":" + key;
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[machineKey];
            if (returnConnectionString(settings))
                return settings.ConnectionString;
            string envKey = GetAppSetting("environment") + ":" + key;
            settings = ConfigurationManager.ConnectionStrings[envKey];
            if (returnConnectionString(settings))
                return settings.ConnectionString;
            settings = ConfigurationManager.ConnectionStrings[key];
            if (returnConnectionString(settings))
                return settings.ConnectionString;
            throw new Exception("No connection string defined");
        }

        private static bool returnConnectionString(ConnectionStringSettings settings)
        {
            if (settings != null)
            {
                if (!string.IsNullOrEmpty(settings.ConnectionString))
                    return true;
            }
            return false;
        }

        public static string GetConnectionString()
        {
            return ConfigUtil.GetConnectionString("connectionString");
        }

        public static string GetConnectionString(string key)
        {
            string connectionString = ReadConnectionString(key);
            if (!connectionString.StartsWith("#"))
                return connectionString;
            return ConfigUtil.Decrypt(connectionString.Substring(1));
        }

        private static string ReadAppSetting(string key)
        {
            string machineName = GetMachineName();

            // search machine:key
            string machineKey = machineName + ":" + key;
            string result = ConfigurationManager.AppSettings[machineKey];
            if (!String.IsNullOrEmpty(result))
                return result;

            // search environment:key 
            string envKeyValue = ConfigurationManager.AppSettings[machineName + ":environment"];
            if (!String.IsNullOrEmpty(envKeyValue))
            {
                string envKey = envKeyValue + ":" + key;
                result = ConfigurationManager.AppSettings[envKey];
                if (!String.IsNullOrEmpty(result))
                    return result;
            }
            // no succes, return plain key vlue
            return ConfigurationManager.AppSettings[key];
        }

        public static string GetAppSetting(string key)
        {
            string appSettingValue = ReadAppSetting(key);
            if (String.IsNullOrEmpty(appSettingValue))
                return appSettingValue;
            if (!appSettingValue.StartsWith("#"))
                return appSettingValue;
            return ConfigUtil.Decrypt(appSettingValue.Substring(1));
        }

        public static Nullable<T> GetAppSetting<T>(string key) where T : struct
        {
            string configValueString = GetAppSetting(key);
            if (configValueString == null)
                return null;
            if (typeof(T) == typeof(bool))
            {
                bool result;
                if (Boolean.TryParse(configValueString, out result))
                    return (result as Nullable<T>);
                else
                    return null;
            }
            else if (typeof(T) == typeof(Int32))
            {
                Int32 result;
                if (Int32.TryParse(configValueString, out result))
                    return (result as Nullable<T>);
                else
                    return null;
            }
            else if (typeof(T) == typeof(Int64))
            {
                Int64 result;
                if (Int64.TryParse(configValueString, out result))
                    return (result as Nullable<T>);
                else
                    return null;
            }
            return null;
        }

        public static string Decrypt(string inputString)
        {
            string majorVersion = "1";
            RijndaelManaged managed = new RijndaelManaged();
            byte[] buffer = Convert.FromBase64String(inputString);
            byte[] rgbSalt = Encoding.ASCII.GetBytes(majorVersion);
            PasswordDeriveBytes bytes = new PasswordDeriveBytes(majorVersion, rgbSalt);
            ICryptoTransform transform = managed.CreateDecryptor(bytes.GetBytes(0x20), bytes.GetBytes(0x10));
            MemoryStream stream = new MemoryStream(buffer);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            byte[] buffer3 = new byte[buffer.Length];
            int count = stream2.Read(buffer3, 0, buffer3.Length);
            stream.Close();
            stream2.Close();
            return Encoding.Unicode.GetString(buffer3, 0, count);
        }

    }
}