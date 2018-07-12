using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CTP
{
    public static class Security
    {
        public static byte[] ComputeHMAC(byte[] key, byte[] data)
        {
            using (var hmac = new HMACSHA256(key))
                return hmac.ComputeHash(data);
        }

        public static byte[] ComputeHMAC(byte[] key, string data)
        {
            using (var hmac = new HMACSHA256(key))
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        public static byte[] CreateSalt(int bytes)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var b = new byte[bytes];
                rng.GetBytes(b);
                return b;
            }
        }

        public static byte[] XOR(byte[] a, byte[] b)
        {
            byte[] rv = new byte[a.Length];
            for (int x = 0; x < a.Length; x++)
            {
                rv[x] = (byte)(a[x] ^ b[x]);
            }
            return rv;
        }

        public static bool SecureEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false; //Should this be secure also?
            int sum = 0;
            for (int x = 0; x < a.Length; x++)
            {
                sum += a[x] ^ b[x];
            }
            return sum == 0;
        }
    }
}
