using System;
using System.Security.Cryptography;
using System.Text;

namespace CTP.SRP
{
    public class SrpUserCredential<T>
    {
        public readonly string UserName;

        public readonly byte[] Verification;

        public readonly byte[] Salt;

        public readonly SrpStrength SrpStrength;

        public readonly T Token;

        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="salt"></param>
        /// <param name="verification"></param>
        /// <param name="srpStrength"></param>
        /// <param name="token"></param>
        public SrpUserCredential(string username, byte[] verification, byte[] salt, SrpStrength srpStrength, T token)
        {
            UserName = username.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            Salt = salt;
            Verification = verification;
            SrpStrength = srpStrength;
            Token = token;
        }

        /// <summary>
        /// Creates a user credential from the provided data.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="strength"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public SrpUserCredential(string username, string password, byte[] salt, SrpStrength strength, T token)
        {
            UserName = username.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            Salt = salt ?? RNG.CreateSalt(64);
            byte[] x = SrpMethods.ComputeX(Salt, UserName, password.Normalize(NormalizationForm.FormKC));
            Verification = SrpMethods.ComputeV(SrpConstants.Lookup(strength), x.ToUnsignedBigInteger()).ToUnsignedByteArray();
            SrpStrength = strength;
            Token = token;
        }
    }
}