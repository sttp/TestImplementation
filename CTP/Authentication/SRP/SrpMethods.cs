using System;
using System.Numerics;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CTP.SRP
{
    public static class SrpMethods
    {
        public static byte[] ComputeV(SrpStrength strength, byte[] x)
        {
            return ComputeV(SrpConstants.Lookup(strength), x.ToUnsignedBigInteger()).ToUnsignedByteArray();
        }

        public static BigInteger ComputeV(SrpConstants param, BigInteger x)
        {
            return param.g.ModPow(x, param.N);
        }

        public static BigInteger ComputeU(int padLength, BigInteger item1, BigInteger item2)
        {
            using (var sha = SHA512.Create())
            {
                return sha.ComputeHash(item1.ToUnsignedByteArray(padLength).Concat(item2.ToUnsignedByteArray(padLength))).ToUnsignedBigInteger();
            }
        }

        public static byte[] ComputeChallenge(BigInteger sessionKey, X509Certificate publicCertificate)
        {
            using (var sha = SHA512.Create())
            {
                return sha.ComputeHash(sessionKey.ToUnsignedByteArray().Concat(publicCertificate?.GetPublicKey()));
            }
        }

        public static byte[] ComputeX(byte[] salt, string secret)
        {
            return ComputeX(salt, Encoding.UTF8.GetBytes(secret));
        }
        public static byte[] ComputeX(byte[] salt, SecureString secret)
        {
            return ComputeX(salt, secret.ToUTF8());
        }

        private static byte[] ComputeX(byte[] salt, byte[] secret)
        {
            try
            {
                using (var sha = SHA512.Create())
                {
                    byte[] outer = new byte[salt.Length + 512 / 8];
                    sha.ComputeHash(secret).CopyTo(outer, salt.Length);
                    salt.CopyTo(outer, 0);
                    return sha.ComputeHash(outer);
                }
            }
            finally
            {
                if (secret != null)
                    Array.Clear(secret, 0, secret.Length);
            }
        }



    }
}
