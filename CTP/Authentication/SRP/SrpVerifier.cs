using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.SRP
{
    public class SrpVerifier
    {
        /// <summary>
        /// The name of the credential record used to authenticate a client.
        /// </summary>
        public readonly string CredentialName;
        /// <summary>
        /// The verifier that can verify a user's secret. This should be kept confidential 
        /// otherwise, an attacker can attempt a brute force attack on the password, 
        /// plus anyone who possesses this verifier can impersonate the server.
        /// </summary>
        public readonly byte[] VerifierCode;
        /// <summary>
        /// A salt value to make verifiers distinct with the same password. 
        /// </summary>
        public readonly byte[] Salt;
        /// <summary>
        /// The bit strength of the SRP authentication. The security strength should be considered equivalent to RSA.
        /// </summary>
        public readonly SrpStrength SrpStrength;
            
        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="credentialName"></param>
        /// <param name="verification"></param>
        /// <param name="salt"></param>
        /// <param name="srpStrength"></param>
        public SrpVerifier(string credentialName, byte[] verification, byte[] salt, SrpStrength srpStrength)
        {
            CredentialName = credentialName.Normalize(NormalizationForm.FormKC).Trim();
            VerifierCode = (byte[])verification.Clone();
            Salt = (byte[])salt.Clone();
            SrpStrength = srpStrength;
        }

        /// <summary>
        /// Creates a verifier from a credential and secret
        /// </summary>
        /// <param name="credentialName"></param>
        /// <param name="secret"></param>
        public SrpVerifier(string credentialName, string secret)
        {
            CredentialName = credentialName.Normalize(NormalizationForm.FormKC).Trim();
            Salt = Security.CreateSalt(64);
            SrpStrength = SrpStrength.Bits2048;
            var pwd = new SrpSecret(Salt, CredentialName, secret);
            VerifierCode = pwd.CreateVerifier(SrpStrength).VerifierCode;
        }

    }

}
