using System;
using System.Security.Cryptography;
using System.Text;

using Konscious.Security.Cryptography;

namespace KingdomApi.Services
{
    public class PasswordHashManager
    {
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            byte[] passwordByteArray = Encoding.UTF8.GetBytes(password);

            var hash = Argon2idHashing(passwordByteArray, salt);

            if (!bool.TryParse(Environment.GetEnvironmentVariable("ENCRYPT_PASSWORD"), out bool isEncryptionEnabled))
            {
                isEncryptionEnabled = false;
            }

            if (isEncryptionEnabled)
            {
                string key = Environment.GetEnvironmentVariable("ENCRYPTION_KEY") ?? @"=z7W%agh0^UuX+^Kvr#PnY=F";
                string encryptedHash = Encrypt(hash, Encoding.UTF8.GetBytes(key));
                return $"{Convert.ToBase64String(salt)}:{encryptedHash}";
            }
            else
            {
                return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
            }
        }

        public static bool VerifyPassword(string password, string hashedAndSalted)
        {
            string[] hashedAndSaltedArray = hashedAndSalted.Split(":");
            string salt = hashedAndSaltedArray[0];

            byte[] saltByteArray = Encoding.UTF8.GetBytes(salt);

            byte[] passwordByteArray = Encoding.UTF8.GetBytes(password);

            var hashedPassword = Argon2idHashing(passwordByteArray, saltByteArray);
            byte[] hash;

            if (hashedAndSaltedArray.Length.Equals(2))
            {
                hash = Encoding.UTF8.GetBytes(hashedAndSaltedArray[1]);
            }
            else
            {
                byte[] ciphertext = Encoding.UTF8.GetBytes(hashedAndSaltedArray[1]);
                byte[] nonce = Encoding.UTF8.GetBytes(hashedAndSaltedArray[2]);
                byte[] tag = Encoding.UTF8.GetBytes(hashedAndSaltedArray[3]);

                string key = Environment.GetEnvironmentVariable("ENCRYPTION_KEY") ?? @"=z7W%agh0^UuX+^Kvr#PnY=F";

                hash = Decrypt(ciphertext, nonce, tag, Encoding.UTF8.GetBytes(key));
            }
            return ByteArraysEqual(hashedPassword, hash);
        }

        private static byte[] Argon2idHashing(byte[] password, byte[] salt)
        {
            var argon2 = new Argon2id(password)
            {
                DegreeOfParallelism = 4, // 2 * number of cores
                MemorySize = 4096, // in KB
                Iterations = 10, // Should take 0.5 seconds
                Salt = salt, // 128 bit recommended
            };

            return argon2.GetBytes(256 / 8); // 128 bit recommended
        }

        private static string Encrypt(byte[] hashedPassaword, byte[] key)
        {
            using var aes = new AesGcm(key);

            var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
            RandomNumberGenerator.Fill(nonce);

            var tag = new byte[AesGcm.TagByteSizes.MaxSize];
            var ciphertext = new byte[hashedPassaword.Length];

            aes.Encrypt(nonce, hashedPassaword, ciphertext, tag);

            return $"{Convert.ToBase64String(ciphertext)}:{Convert.ToBase64String(nonce)}:{Convert.ToBase64String(tag)}";
        }

        private static byte[] Decrypt(byte[] ciphertext, byte[] nonce, byte[] tag, byte[] key)
        {
            using var aes = new AesGcm(key);

            var plaintextBytes = new byte[ciphertext.Length];

            aes.Decrypt(nonce, ciphertext, tag, plaintextBytes);

            return plaintextBytes;

        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            bool areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= a[i] == b[i];
            }
            return areSame;
        }

    }
}
