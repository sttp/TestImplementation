using System;
using System.Security.Cryptography;
using System.Text;

namespace CTP.SRP
{
    public class SrpCredential<T>
    {
        public readonly SrpVerifier Verifier;

        public readonly T Token;

        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="credentialName"></param>
        /// <param name="salt"></param>
        /// <param name="verifierCode"></param>
        /// <param name="srpStrength"></param>
        /// <param name="token"></param>
        public SrpCredential(string credentialName, byte[] verifierCode, byte[] salt, SrpStrength srpStrength, T token)
        {
            Verifier = new SrpVerifier(credentialName, verifierCode, salt, srpStrength);
            Token = token;
        }

        /// <summary>
        /// Creates a credential from the provided data.
        /// </summary>
        /// <param name="credentialName"></param>
        /// <param name="secret"></param>
        /// <param name="salt"></param>
        /// <param name="strength"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public SrpCredential(string credentialName, string secret, byte[] salt, SrpStrength strength, T token)
        {
            var s = new SrpSecret(salt, credentialName, secret);
            Verifier = s.CreateVerifier(strength);
            Token = token;
        }
    }
}