using System;
using System.IO;
using System.Security.Cryptography;

namespace decryptor
{
    class MainClass
    {
        public static byte[] PasswordBytes =
        {
            (byte) 12,
            (byte) 189,
            (byte) 97,
            (byte) 37,
            (byte) 25,
            (byte) 95,
            (byte) 234,
            (byte) 58,
            (byte) 103,
            (byte) 19,
            (byte) 197,
            (byte) 120,
            (byte) 0,
            (byte) 75,
            (byte) 108,
            (byte) 248
          };
        public static byte[] salt =
        {
          (byte) 37,
          (byte) 13,
          (byte) 135,
          (byte) 65,
          (byte) 173,
          (byte) 115,
          (byte) 192,
          (byte) 87
        };

        public static void Main(string[] args)
        {
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(PasswordBytes, 
                                                             salt, 
                                                             1000);
            RijndaelManaged rijAlg = new RijndaelManaged();
            rijAlg.KeySize = 256;
            rijAlg.BlockSize = 128;
            rijAlg.Mode = CipherMode.CBC;
            rijAlg.Key = key.GetBytes(256 / 8);
            rijAlg.IV = key.GetBytes(128 / 8);
            String line;
            using (StreamReader sr = new StreamReader("res.txt"))
            {
                // Read the stream to a string, and write the string to the console.
                line = sr.ReadToEnd();
                Console.WriteLine(line);
            }
            byte[] data = Convert.FromBase64String(line);
            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
            byte[] plainText2 = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                }
                plainText2 = ms.ToArray();
            }

            Console.WriteLine(plainText2);
            File.WriteAllBytes("obj.bin", plainText2);
        }
    }
}
