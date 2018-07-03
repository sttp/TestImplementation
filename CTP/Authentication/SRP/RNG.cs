using System.Numerics;
using System.Security.Cryptography;

namespace CTP.SRP
{
    public static class RNG
    {
        public static byte[] CreateSalt(int bytes)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var b = new byte[bytes];
                rng.GetBytes(b);
                return b;
            }
        }
    }
}