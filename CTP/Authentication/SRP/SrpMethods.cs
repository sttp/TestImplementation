using System;
using System.Numerics;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CTP.SRP
{
    internal static class SrpMethods
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

        public static byte[] ComputeX(byte[] salt, string identifier, string secret)
        {
            return ComputeX(salt, identifier, Encoding.UTF8.GetBytes(secret));
        }
        public static byte[] ComputeX(byte[] salt, string identifier, SecureString secret)
        {
            return ComputeX(salt, identifier, secret.ToUTF8());
        }

        private static byte[] ComputeX(byte[] salt, string identifier, byte[] secret)
        {
            byte[] inner = null;
            try
            {
                using (var sha = SHA512.Create())
                {
                    byte[] idBytes = Encoding.UTF8.GetBytes(identifier);
                    inner = new byte[idBytes.Length + 1 + secret.Length];
                    byte[] outer = new byte[salt.Length + 512 / 8];
                    idBytes.CopyTo(inner, 0);
                    inner[idBytes.Length] = (byte)':';
                    secret.CopyTo(inner, idBytes.Length + 1);
                    sha.ComputeHash(inner).CopyTo(outer, salt.Length);
                    salt.CopyTo(outer, 0);

                    return sha.ComputeHash(outer);
                }
            }
            finally
            {
                if (secret != null)
                    Array.Clear(secret, 0, secret.Length);
                if (inner != null)
                    Array.Clear(inner, 0, inner.Length);
            }
        }



    }
}
