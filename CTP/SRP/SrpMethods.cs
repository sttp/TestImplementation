using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CTP.SRP
{
    internal static class SrpMethods
    {
        public static byte[] ComputeX(byte[] salt, string identifier, string password)
        {
            int identifierUtfLen = Encoding.UTF8.GetByteCount(identifier);
            int passwordUtfLen = Encoding.UTF8.GetByteCount(password);

            byte[] inner = new byte[identifierUtfLen + 1 + passwordUtfLen];
            byte[] outer = new byte[salt.Length + 512 / 8];

            Encoding.UTF8.GetBytes(identifier, 0, identifier.Length, inner, 0);
            inner[identifierUtfLen] = (byte)':';
            Encoding.UTF8.GetBytes(password, 0, password.Length, inner, identifierUtfLen + 1);

            byte[] x;
            using (var sha = SHA512.Create())
            {
                sha.ComputeHash(inner).CopyTo(outer, salt.Length);
                salt.CopyTo(outer, 0);
                x = sha.ComputeHash(outer);
            }
            return x;
        }

        internal static BigInteger ComputeV(SrpConstants param, BigInteger x)
        {
            return param.g.ModPow(x, param.N);
        }

        internal static BigInteger ComputeU(int padLength, BigInteger item1, BigInteger item2)
        {
            using (var sha = SHA512.Create())
            {
                return sha.ComputeHash(item1.ToUnsignedByteArray(padLength)
                                            .Concat(item2.ToUnsignedByteArray(padLength))).ToUnsignedBigInteger();
            }
        }

        internal static byte[] ComputeChallenge(byte challengeType, BigInteger sessionKey, X509Certificate clientCertificate, X509Certificate serverCertificate)
        {
            using (var sha = SHA512.Create())
            {
                return sha.ComputeHash(new byte[] { challengeType }.Concat(sessionKey.ToUnsignedByteArray()
                                            , clientCertificate?.GetSerialNumber()
                                            , serverCertificate?.GetSerialNumber()));
            }
        }



    }
}
