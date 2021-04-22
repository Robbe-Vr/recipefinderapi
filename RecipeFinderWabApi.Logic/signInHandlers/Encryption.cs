using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RecipeFinderWabApi.Logic
{
    public class Encryption
    {
        public EncryptionObject HashString(EncryptionObject encObj)
        {
            HashAlgorithm algorithm = SHA256.Create();

            string saltedString = SaltOnString(encObj.Text, encObj.Salt);

            byte[] hashBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltedString));

            foreach (byte b in hashBytes)
            {
                encObj.Result += b.ToString("X2");
            }

            return encObj;
        }

        private string SaltOnString(string str, string saltString)
        {
            byte[] saltedBytes = Encoding.UTF8.GetBytes(str);

            byte[] salt = Encoding.UTF8.GetBytes(saltString);

            for (int i = 0; i < salt.Length; i++)
            {
                saltedBytes.Append(salt[i]);
            }

            string saltedString = "";
            foreach (byte b in saltedBytes)
            {
                saltedString += b.ToString("X2");
            }

            return saltedString;
        }

        public string CreateSalt(int size)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            byte[] buffer = new byte[size];

            rng.GetBytes(buffer);

            return Convert.ToBase64String(buffer);
        }

    }
}
