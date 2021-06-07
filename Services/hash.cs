using System;
using System.Security.Cryptography;
using System.Text;

using Konscious.Security.Cryptography;

namespace KingdomApi.Services
{
    public class Hash
    {
        public static String hashPassword()
        {
          Byte[] salt = new Byte[16];
          using (var rng = RandomNumberGenerator.Create())
          {
              rng.GetBytes(salt);
          }
          Byte[] password = Encoding.UTF8.GetBytes("password");
          var argon2 = new Argon2id(password);

          argon2.DegreeOfParallelism = 16; // 2 * number of cors
          argon2.MemorySize = 1048576; // 1GB
          argon2.Iterations = 10; // Should take 0.5 seconds
          argon2.Salt = salt; // 128 bit recommended

          var hash = argon2.GetBytes(32); // 128 bit recommended

          return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        private static (byte[] ciphertext, byte[] nonce, byte[] tag) Encrypt(string plaintext, byte[] key)
        {
            using (var aes = new AesGcm(key))
            {
                var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
                RandomNumberGenerator.Fill(nonce);

                var tag = new byte[AesGcm.TagByteSizes.MaxSize];

                var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
                var ciphertext = new byte[plaintextBytes.Length];

                aes.Encrypt(nonce, plaintextBytes, ciphertext, tag);

                return (ciphertext, nonce, tag);
            }
        }

        private static string Decrypt(byte[] ciphertext, byte[] nonce, byte[] tag, byte[] key)
        {
            using (var aes = new AesGcm(key))
            {
                var plaintextBytes = new byte[ciphertext.Length];

                aes.Decrypt(nonce, ciphertext, tag, plaintextBytes);

                return Encoding.UTF8.GetString(plaintextBytes);
            }
        }

    }
}