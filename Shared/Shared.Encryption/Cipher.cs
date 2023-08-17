using System;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Encryption
{
    public class Cipher
    {
        private static byte[] DeriveKeyFromPassword(string password)
        {
            var emptySalt = Array.Empty<byte>();
            var iterations = 1000;
            var desiredKeyLength = 16; // 16 bytes equal 128 bits.
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
                                            emptySalt,
                                            iterations,
                                            hashMethod,
                                            desiredKeyLength);
        }
        private static byte[] IV = new byte[16];
        public static async Task<string> EncryptAsync(string clearText)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword("PasswordHere");
            aes.IV = IV;
            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(clearText));
            await cryptoStream.FlushFinalBlockAsync();
            return Convert.ToBase64String(output.ToArray());
        }

        public static async Task<string> DecryptAsync(string encrypted)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword("PasswordHere");
            aes.IV = IV;
            using MemoryStream input = new(Convert.FromBase64String(encrypted));
            using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            await cryptoStream.CopyToAsync(output);
            return Encoding.Unicode.GetString(output.ToArray());
        }
    }
}
