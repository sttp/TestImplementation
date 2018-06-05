using System;
using System.Security.Cryptography;
using System.Text;

namespace CTP.SRP
{

    public class SrpUserCredential<T>
    {
        public readonly SrpVerifier Verifier;

        public readonly T Token;

        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="salt"></param>
        /// <param name="verification"></param>
        /// <param name="srpStrength"></param>
        /// <param name="token"></param>
        public SrpUserCredential(string username, byte[] verification, byte[] salt, SrpStrength srpStrength, int iterations, T token)
        {
            Verifier = new SrpVerifier(username.Normalize(NormalizationForm.FormKC).Trim(), verification, salt, srpStrength, iterations);
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
        public SrpUserCredential(string username, string password, byte[] salt, SrpStrength strength, int iterations, T token)
        {
            var pwd = new SrpPassword(salt, username, password, iterations);
            Verifier = pwd.CreateVerifier(strength);
            Token = token;
        }
    }
}