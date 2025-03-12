using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Kharazmi.AspNetMvc.Core.Helpers
{
    public class HashKey
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public static class HashHelper
    {
        public static string HashPassword(string password, int numberOfRounds = 1000)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            byte[] saltBytes;
            byte[] hashedPasswordBytes;
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 16, numberOfRounds))
            {
                saltBytes = rfc2898DeriveBytes.Salt;
                hashedPasswordBytes = rfc2898DeriveBytes.GetBytes(32);
            }

            var outArray = new byte[49];
            Buffer.BlockCopy(saltBytes, 0, outArray, 1, 16);
            Buffer.BlockCopy(hashedPasswordBytes, 0, outArray, 17, 32);
            return Convert.ToBase64String(outArray);
        }

        public static string BuildCacheKey(HashSet<HashKey> hashKeys)
        {
            var hashCode = GetHashCode(hashKeys);
            return string.Format(CultureInfo.InvariantCulture, "hashCode_{0}", hashCode);
        }

        public static string BuildCacheKey(string key)
        {
            var hashCode = GetHashCode(
                new HashSet<HashKey>
                {
                    new HashKey
                    {
                        Id = Guid.NewGuid(), Name = key
                    }
                });
            return string.Format(CultureInfo.InvariantCulture, "hashCode_{0}", hashCode);
        }

        public static string HashMd5(string key)
        {
            var keyHash = Encoding.UTF8.GetBytes(key);
            var hmac = new HMACMD5(keyHash);
            var hashBytes = hmac.ComputeHash(keyHash);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private static int GetHashCode(IEnumerable<HashKey> permissions)
        {
            unchecked
            {
                var hash = 17;
                foreach (var permission in permissions.OrderBy(p => p.Id)) hash = hash * 23 + permission.GetHashCode();

                return hash;
            }
        }

        public static string CreateCodeVerifier(int lenght)
        {
            var rng = RandomNumberGenerator.Create();

            var bytes = new byte[lenght];
            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}