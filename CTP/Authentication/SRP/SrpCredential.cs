using System;
using System.Security.Cryptography;
using System.Text;

namespace CTP.SRP
{
    public class SrpCredential
    {
        public uint CredentialNameID;

        public readonly SrpVerifier Verifier;

        public readonly string LoginName;

        public readonly string[] Roles;

        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="credentialNameID"></param>
        /// <param name="credentialName"></param>
        /// <param name="salt"></param>
        /// <param name="verifierCode"></param>
        /// <param name="srpStrength"></param>
        /// <param name="loginName"></param>
        /// <param name="roles"></param>
        public SrpCredential(uint credentialNameID, string credentialName, byte[] verifierCode, byte[] salt, SrpStrength srpStrength, string loginName, string[] roles)
        {
            CredentialNameID = credentialNameID;
            Verifier = new SrpVerifier(credentialName, verifierCode, salt, srpStrength);
            LoginName = loginName;
            Roles = roles;
        }

        /// <summary>
        /// Creates a credential from the provided data.
        /// </summary>
        /// <param name="credentialNameID"></param>
        /// <param name="credentialName"></param>
        /// <param name="secret"></param>
        /// <param name="salt"></param>
        /// <param name="strength"></param>
        /// <param name="loginName"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public SrpCredential(uint credentialNameID, string credentialName, string secret, byte[] salt, SrpStrength strength, string loginName, string[] roles)
        {
            CredentialNameID = credentialNameID;
            var s = new SrpSecret(salt, credentialName, secret);
            Verifier = s.CreateVerifier(strength);
            LoginName = loginName;
            Roles = roles;
        }
    }
}