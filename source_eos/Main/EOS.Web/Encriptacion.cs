using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Security.Cryptography;

namespace EOS.Web
{
  public class Encriptacion
  {

    private static string EncryptAES(string plainText,
                                 string passPhrase,
                                 string saltValue,
                                 string hashAlgorithm,
                                 int passwordIterations,
                                 string initVector,
                                 int keySize)
    {
      Byte[] initVectorBytes;
      initVectorBytes = Encoding.ASCII.GetBytes(initVector);

      Byte[] saltValueBytes;
      saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

      Byte[] plainTextBytes;
      plainTextBytes = Encoding.UTF8.GetBytes(plainText);

      PasswordDeriveBytes password;
      password = new PasswordDeriveBytes(passPhrase,
                                         saltValueBytes,
                                         hashAlgorithm,
                                         passwordIterations);

      Byte[] keyBytes;
      keyBytes = password.GetBytes(keySize / 8);


      RijndaelManaged symmetricKey;
      symmetricKey = new RijndaelManaged();

      symmetricKey.Mode = CipherMode.CBC;

      ICryptoTransform encryptor;
      encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

      MemoryStream memoryStream;
      memoryStream = new MemoryStream();

      CryptoStream cryptoStream;
      cryptoStream = new CryptoStream(memoryStream,
                                      encryptor,
                                      CryptoStreamMode.Write);
      cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

      cryptoStream.FlushFinalBlock();

      Byte[] cipherTextBytes;
      cipherTextBytes = memoryStream.ToArray();

      memoryStream.Close();
      cryptoStream.Close();

      string cipherText;
      cipherText = Convert.ToBase64String(cipherTextBytes);

      return cipherText;

    }

    private static string DecryptAES(string cipherText,
                                     string passPhrase,
                                     string saltValue,
                                     string hashAlgorithm,
                                     int passwordIterations,
                                     string initVector,
                                     int keySize) 
{

    Byte[] initVectorBytes;
    initVectorBytes = Encoding.ASCII.GetBytes(initVector);

    Byte[] saltValueBytes;
    saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

    Byte[] cipherTextBytes;
    cipherTextBytes = Convert.FromBase64String(cipherText);

    PasswordDeriveBytes password;
    password = new PasswordDeriveBytes(passPhrase, 
                                       saltValueBytes, 
                                       hashAlgorithm, 
                                       passwordIterations);

    Byte[] keyBytes;
    keyBytes = password.GetBytes(keySize / 8);

    RijndaelManaged symmetricKey;
    symmetricKey = new RijndaelManaged();

    symmetricKey.Mode = CipherMode.CBC;

    ICryptoTransform decryptor;
    decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

    MemoryStream memoryStream;
    memoryStream = new MemoryStream(cipherTextBytes);

    CryptoStream cryptoStream;
    cryptoStream = new CryptoStream(memoryStream, 
                                    decryptor, 
                                    CryptoStreamMode.Read);

    Byte[] plainTextBytes = new Byte[cipherTextBytes.Length];

    int decryptedByteCount;
    decryptedByteCount = cryptoStream.Read(plainTextBytes, 
                                           0, 
                                           plainTextBytes.Length);

    memoryStream.Close();
    cryptoStream.Close();

    string plainText;
    plainText = Encoding.UTF8.GetString(plainTextBytes, 
                                        0, 
                                        decryptedByteCount);

    return plainText;
}


    public static string Encrypt(string plainText, string passPhrase)
    {

      string hashAlgorithm;
      int passwordIterations;
      string initVector;
      int keySize;

      passPhrase = passPhrase.ToUpper();
      hashAlgorithm = "MD5";                     // can be "MD5" or "SHA1"
      passwordIterations = 2;                    // can be any number
      initVector = passPhrase.PadRight(16, '_'); // must be 16 bytes
      keySize = 256;                             // can be 192 or 128

      try
      {
        string rtv = EncryptAES(plainText,
                                            passPhrase,
                                            passPhrase,
                                            hashAlgorithm,
                                            passwordIterations,
                                            initVector,
                                            keySize);
        return rtv;
      }
      catch (System.Exception ex)
      {
        return "";
      }
    }

    public static string Decrypt(string cipherText, string passPhrase)
    {

      string hashAlgorithm;
      int passwordIterations;
      string initVector;
      int keySize;

      passPhrase = passPhrase.ToUpper();
      hashAlgorithm = "MD5";                     // can be "MD5" or "SHA1"
      passwordIterations = 2;                    // can be any number
      initVector = passPhrase.PadRight(16, '_'); // must be 16 bytes
      keySize = 256;                             // can be 192 or 128

      if (cipherText == "") return "";

      try
      {
        string rtv = DecryptAES(cipherText,
                                            passPhrase,
                                            passPhrase,
                                            hashAlgorithm,
                                            passwordIterations,
                                            initVector,
                                            keySize);
        return rtv;
      }
      catch (System.Exception ex)
      {
        return "";
      }
    }

  }
}
