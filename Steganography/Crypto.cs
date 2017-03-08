using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Steganography
{
    public class Crypto
    {
        private static byte[] _s = Encoding.ASCII.GetBytes("nitesh1234rite456756mnklhjgk");

        /// <summary>
        /// Encrypt the given string using AES_Enc.  The string can be decrypted using 
        /// AES_Dec().  The s_key parameters must match.
        /// </summary>
        /// <param name="msgTxt">The text to encrypt.</param>
        /// <param name="s_key">A password used to generate a s_key for encryption.</param>
        public static string AES_Enc(string msgTxt, string s_key)
        {
            if (string.IsNullOrEmpty(msgTxt))
                throw new ArgumentNullException("msgTxt");
            if (string.IsNullOrEmpty(s_key))
                throw new ArgumentNullException("s_key");

            string stringOutput = null;                       // Encrypted string to return
            RijndaelManaged algorithmAES = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the s_key from the shared secret and the salt
                Rfc2898DeriveBytes k = new Rfc2898DeriveBytes(s_key, _s);

                // Create a RijndaelManaged object
                algorithmAES = new RijndaelManaged();
                algorithmAES.Key = k.GetBytes(algorithmAES.KeySize / 8);

                // Create a ict_d to perform the stream transform.
                ICryptoTransform ict_e = algorithmAES.CreateEncryptor(algorithmAES.Key, algorithmAES.IV);

                // Create the streams used for encryption.
                using (MemoryStream ms_e = new MemoryStream())
                {
                    // prepend the IV
                    ms_e.Write(BitConverter.GetBytes(algorithmAES.IV.Length), 0, sizeof(int));
                    ms_e.Write(algorithmAES.IV, 0, algorithmAES.IV.Length);
                    using (CryptoStream cs_e = new CryptoStream(ms_e, ict_e, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw_e = new StreamWriter(cs_e))
                        {
                            //Write all data to the stream.
                            sw_e.Write(msgTxt);
                        }
                    }
                    stringOutput = Convert.ToBase64String(ms_e.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (algorithmAES != null)
                    algorithmAES.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return stringOutput;
        }

        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using 
        /// AES_Enc(), using an identical s_key.
        /// </summary>
        /// <param name="c_Text">The text to decrypt.</param>
        /// <param name="s_key">A password used to generate a s_key for decryption.</param>
        public static string AES_Dec(string c_Text, string s_key)
        {
            if (string.IsNullOrEmpty(c_Text))
                throw new ArgumentNullException("c_Text");
            if (string.IsNullOrEmpty(s_key))
                throw new ArgumentNullException("s_key");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged algorithmAES = null;

            // Declare the string used to hold
            // the decrypted text.
            string msg_txt = null;

            try
            {
                // generate the s_key from the shared secret and the salt
                Rfc2898DeriveBytes k = new Rfc2898DeriveBytes(s_key, _s);

                // Create the streams used for decryption.                
                byte[] bytes = Convert.FromBase64String(c_Text);
                using (MemoryStream ms_d = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object
                    // with the specified s_key and IV.
                    algorithmAES = new RijndaelManaged();
                    algorithmAES.Key = k.GetBytes(algorithmAES.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    algorithmAES.IV = r_Abyte(ms_d);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform ict_d = algorithmAES.CreateDecryptor(algorithmAES.Key, algorithmAES.IV);
                    using (CryptoStream cs_d = new CryptoStream(ms_d, ict_d, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr_d = new StreamReader(cs_d))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            msg_txt = sr_d.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (algorithmAES != null)
                    algorithmAES.Clear();
            }

            return msg_txt;
        }

        private static byte[] r_Abyte(Stream sm)
        {
            byte[] r_len = new byte[sizeof(int)];
            if (sm.Read(r_len, 0, r_len.Length) != r_len.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] bfr = new byte[BitConverter.ToInt32(r_len, 0)];
            if (sm.Read(bfr, 0, bfr.Length) != bfr.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return bfr;
        }
    }
}
