using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Steganography
{
    class DES
    {
        //Used as initialization vector
        static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("KushagraAhuja");
               

        /*The function is for encrypting the plain text using the key provided via DES algorithm.
         * Function accepts two parameters one of string type and one of byte type:
         * 1. orignalString that is the Plain Text.
         * 2. key that is the key used for decrypting.
         * The function returns a string type parameter i.e., the encrypted text.  */
        public static string Encrypt(string originalString, byte[] key)
        {
            //Check whether the string entered by User is not empty.
            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException("The string which needs to be encrypted can not be null.");
            }
            //object of DESCryptoServiceProvider created.
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            //memory stream object created.
            MemoryStream memoryStream = new MemoryStream();
            //crypto stream created and memory stream and DES service provier as parameters and key and byte as key and initialization vector.
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(key, bytes), CryptoStreamMode.Write);
            //object of stream writer created and the encryped text is flushed
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            //the encrypted text is returned via the memorystream
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        /*The function is for decrypting the encrypted text using the key provided via DES algorithm.
         * Function accepts two parameters one of string type and one of byte type:
         * 1. cryptedString that is the Encrypted Text.
         * 2. key that is the key used for encrypting.
         * The function returns a string type parameter i.e., the encrypted text.  */
        public static string Decrypt(string cryptedString, byte[] key)
        {
            //Check for the empty string
            if (String.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
            }
            //object of DESCryptoServiceProvider created
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            //memory stream object created and encrypted sring passed as parameter
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
            //object of crypto stream created
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(key, bytes), CryptoStreamMode.Read);
            //streamreader reades the olain text
            StreamReader reader = new StreamReader(cryptoStream);
            //plain text returned.
            return reader.ReadToEnd();
        }

    }
}
