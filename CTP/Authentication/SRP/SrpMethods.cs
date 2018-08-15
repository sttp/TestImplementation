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
        public static BigInteger ComputeV(SrpConstants param, byte[] clientPublicKey, byte[] serverPublicKey, string secret)
        {
            var x = ComputeX(clientPublicKey, serverPublicKey, secret);
            return param.g.ModPow(x, param.N);
        }

        public static BigInteger ComputeU(int padLength, BigInteger item1, BigInteger item2)
        {
            using (var sha = SHA512.Create())
            {
                return sha.ComputeHash(item1.ToUnsignedByteArray(padLength).Concat(item2.ToUnsignedByteArray(padLength))).ToUnsignedBigInteger();
            }
        }

        public static BigInteger ComputePublicB(SrpConstants param, BigInteger privateB, BigInteger verifier)
        {
            return param.k.ModMul(verifier, param.N).ModAdd(param.g.ModPow(privateB, param.N), param.N);
        }

        public static byte[] ComputeSessionKey(SrpConstants param, BigInteger publicA, BigInteger verifier, BigInteger u, BigInteger privateB)
        {
            return publicA.ModMul(verifier.ModPow(u, param.N), param.N).ModPow(privateB, param.N).ToUnsignedByteArray();
        }

        public static byte[] ComputeSessionKey(SrpConstants param, BigInteger u, BigInteger x, BigInteger publicB, BigInteger privateA, BigInteger verifier)
        {
            var exp1 = privateA.ModAdd(u.ModMul(x, param.N), param.N);
            var base1 = publicB.ModSub(param.k.ModMul(verifier, param.N), param.N);
            return base1.ModPow(exp1, param.N).ToUnsignedByteArray();
        }

        public static BigInteger ComputeX(byte[] clientPublicKey, byte[] serverPublicKey, string secret)
        {
            byte[] secB = Encoding.UTF8.GetBytes(secret);
            using (var sha = SHA512.Create())
            {
                byte[] outer = new byte[512 / 8 + clientPublicKey.Length + serverPublicKey.Length];
                sha.ComputeHash(secB).CopyTo(outer, 0);
                clientPublicKey.CopyTo(outer, 512 / 8);
                serverPublicKey.CopyTo(outer, 512 / 8 + clientPublicKey.Length);
                return sha.ComputeHash(outer).ToUnsignedBigInteger();
            }
        }

    }
}
