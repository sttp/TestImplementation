using System;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace CTP.SRP
{
    public class SrpUserCredential
    {
        public readonly string UserName;

        public readonly byte[] Verification;

        public readonly byte[] Salt;

        public readonly SrpStrength SrpStrength;

        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="salt"></param>
        /// <param name="verification"></param>
        /// <param name="srpStrength"></param>
        public SrpUserCredential(string username, byte[] verification, byte[] salt, SrpStrength srpStrength)
        {
            UserName = username.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            Salt = salt;
            Verification = verification;
            SrpStrength = srpStrength;
        }

        /// <summary>
        /// Creates a user credential from the provided data.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="strength"></param>
        /// <returns></returns>
        public SrpUserCredential(string username, string password, byte[] salt = null, SrpStrength strength = SrpStrength.Bits1024)
        {
            UserName = username.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            Salt = salt ?? RNG.CreateSalt(64);
            Verification = ComputeVerifier(SrpConstants.Lookup(strength), Salt, username, password.Normalize(NormalizationForm.FormKC));
            SrpStrength = strength;
        }

        private static byte[] ComputeVerifier(SrpConstants param, byte[] salt, string identifier, string password)
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

            return param.g.ModPow(x.ToUnsignedBigInteger(), param.N).ToUnsignedByteArray();
        }
    }
}