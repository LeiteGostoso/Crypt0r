///<introduction .
///
///     Everytime I use any first person noun, such as 'I', 'me' or 'my', it will be
///     refering to the developer of this encrypting system.
/// 
///     This AES encryptor was made by Lucas Rodrigues (using the placeholder of
///     Roberto).
///     
///     Please don't distribute this without crediting the developer.
///
///     Made for education purposes only. If you use this for any other purpose,
///     I won't be responsible.
///
///     When you create an instance, a random key and iv will be generated.
///     If you set them manually but with incorrect lengths, the encrypting system
///     will generate a random one for you. Keep calm.
///
///     Be careful! Test it on a virtual machine only! If your PC gets f*cked, it's
///     not my fault.
///     
///     If you use this for any other purpose but education and personal use,
///     DON'T credit the developer. Even if you do, he won't be responsible for
///     anything.
/// />
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace Crypt0r
{
    public class Crypt0rV3
    {
        public string Key { get; set; }
        public string IV { get; set; }
        private bool IsRegistred = false;

        /// <summary>
        /// Creates an instance of Crypt0rV3 class.
        /// </summary>
        /// <param name="Key">It HAS to be 32 characters long.</param>
        /// <param name="IV">It HAS to be 16 characters long.</param>
        public Crypt0rV3(string Key, string IV)
        {
            if (Key.Length != 32)
                GenerateKey();
            else this.Key = Key;
            if (IV.Length != 16)
                GenerateIV();
            else this.IV = IV;
        }

        public Crypt0rV3()
        {
            Key = GenerateKey();
            IV = GenerateIV();
        }

        public void SetInRegistry()
        {
            if (!IsRegistred)
            {
                RegistryKey keyRegistry = Registry.CurrentUser.CreateSubKey("GITGUD");
                keyRegistry.SetValue("Key", Key);
                keyRegistry.SetValue("IV", IV);
            }
        }

        public void Destroy()
        {
            DeleteFromRegistry();
        }

        private void DeleteFromRegistry()
        {
            string[] rk = Registry.CurrentUser.GetSubKeyNames();
            string allKeys = "";
            foreach (string key in rk)
                allKeys += key + '.';
            if (allKeys.Contains("GITGUD"))
                Registry.CurrentUser.DeleteSubKey("GITGUD");
            else throw new Exception("Failed to destroy.");
        }

        public static string GenerateKey()
        {
            Random r = new Random();
            List<int> PossibleChars = new List<int>();
            for (int i = 65; i <= 90; i++)
                PossibleChars.Add(i);
            for (int i = 97; i <= 122; i++)
                PossibleChars.Add(i);
            for (int i = 48; i <= 57; i++)
                PossibleChars.Add(i);
            string Key = "";
            for (int i = 0; i < 32; i++)
            {
                int randomIndex = r.Next(PossibleChars.Count);
                Key += (char)PossibleChars[randomIndex];
            }
            this.Key = Key;
        }

        public static string GenerateIV()
        {
            Random r = new Random();
            List<int> PossibleChars = new List<int>();
            for (int i = 90; i >= 65; i--)
                PossibleChars.Add(i);
            for (int i = 122; i >= 97; i--)
                PossibleChars.Add(i);
            for (int i = 57; i >= 48; i--)
                PossibleChars.Add(i);
            string IV = "";
            for (int i = 0; i < 16; i++)
            {
                int randomIndex = r.Next(PossibleChars.Count);
                IV += (char)PossibleChars[randomIndex];
            }
            this.IV = IV;
        }

        public byte[] EncodeBytes(byte[] bytesToEncode)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = 256,
                Key = Encoding.UTF8.GetBytes(Key),
                IV = Encoding.UTF8.GetBytes(IV),
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC
            };
            ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
            bytesToEncode = crypto.TransformFinalBlock(bytesToEncode, 0, bytesToEncode.Length);
            crypto.Dispose();
            return bytesToEncode;
        }

        public byte[] DecodeBytes(byte[] encryptedBytes)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = 256,
                Key = Encoding.UTF8.GetBytes(Key),
                IV = Encoding.UTF8.GetBytes(IV),
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC
            };
            ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] secret = crypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            crypto.Dispose();
            return secret;
        }
    }
}
