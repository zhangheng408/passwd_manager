using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace MM
{
    public class AES
    {
         private byte[] IV;
        private byte[] Key;
        public AES(String k)
        {
            IV = trans(Encoding.ASCII.GetBytes("123456"), 16);
            Key = trans(Encoding.ASCII.GetBytes(k), 32);
        }
        public void setKey(String k)
        {
            Key = trans(Encoding.ASCII.GetBytes(k), 32);
        }
        private byte[] trans(byte[] bs, int to)
        {
            for (int i = 0; i < bs.Length; i++)
            {
                bs[i] = (byte)((bs[i] - 33) / 93.0 * 256);
            }

            if (bs.Length == to)
            {
                return bs;
            }
            else if (bs.Length > to)
            {
                byte[] ts = new byte[to];
                for (int i = 0; i < ts.Length; i++)
                {
                    ts[i] = bs[i];
                }
                for (int i = ts.Length; i < bs.Length; i++)
                {
                    ts[i % ts.Length] = (byte)(bs[i] * ts[i % ts.Length]);
                }
                return ts;
            }
            else
            {
                byte[] ts = new byte[to];
                if (bs.Length < 2)
                {
                    Random rd = new Random(DateTime.Now.Second);
                    for (int i = 0; i < bs.Length; i++)
                    {
                        ts[i] = (byte)rd.Next(128);
                    }
                    return ts;
                }
                for (int i = 0; i < bs.Length; i++)
                {
                    ts[i] = bs[i];
                }
                for (int i = bs.Length; i < ts.Length; i++)
                {
                    ts[i] = (byte)(bs[i % bs.Length] * bs[i % (bs.Length - 1)]);
                }
                return ts;
            }
        }
        public string Encrypt(string plainText)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            String encrypted;
            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = BitConverter.ToString(msEncrypt.ToArray());
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }
        private byte[] getbyte(String s)
        {
            s = s.Replace("-", "");
            byte[] b = new byte[s.Length / 2];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = Convert.ToByte(s.Substring(i * 2, 2), 16);
            }
            return b;
        }
        public string Decrypt(String cipher)
        {
            byte[] cipherText = getbyte(cipher);
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // Create an AesCryptoServiceProvider object
                // with the specified key and IV.
                using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {

                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                }
            }
            catch
            {
                plaintext = "????";
            }

            return plaintext;

        }
    }
}
