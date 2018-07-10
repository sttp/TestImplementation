using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CTP.SRP
{
    internal static class SrpMethods
    {
        internal static byte[] ComputeV(SrpStrength strength, byte[] x)
        {
            return ComputeV(SrpConstants.Lookup(strength), x.ToUnsignedBigInteger()).ToUnsignedByteArray();
        }

        internal static BigInteger ComputeV(SrpConstants param, BigInteger x)
        {
            return param.g.ModPow(x, param.N);
        }

        internal static BigInteger ComputeU(int padLength, BigInteger item1, BigInteger item2)
        {
            using (var sha = SHA512.Create())
            {
                return sha.ComputeHash(item1.ToUnsignedByteArray(padLength).Concat(item2.ToUnsignedByteArray(padLength))).ToUnsignedBigInteger();
            }
        }

        internal static byte[] ComputeChallenge(BigInteger sessionKey, X509Certificate publicCertificate)
        {
            using (var sha = SHA512.Create())
            {
                return sha.ComputeHash(sessionKey.ToUnsignedByteArray().Concat(publicCertificate?.GetPublicKey()));
            }
        }



    }
}
