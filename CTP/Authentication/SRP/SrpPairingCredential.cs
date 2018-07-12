using System;

namespace CTP.SRP
{
    public class SrpPairingCredential
    {
        public readonly SrpVerifier Verifier;

        public readonly DateTime ExpireTime;

        public readonly string AssignedCredentialName;

        public readonly string LoginName;

        public readonly string[] Roles;


        /// <summary>
        /// Creates a user credential from the provided data.
        /// </summary>
        /// <param name="pairingName"></param>
        /// <param name="paringPin"></param>
        /// <param name="expireTime"></param>
        /// <param name="assignedCredentialName"></param>
        /// <param name="loginName"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public SrpPairingCredential(string pairingName, string paringPin, DateTime expireTime, string assignedCredentialName, string loginName, string[] roles)
        {
            Verifier = new SrpVerifier(pairingName, paringPin);
            ExpireTime = expireTime;
            AssignedCredentialName = assignedCredentialName;
            LoginName = loginName;
            Roles = roles;
        }
    }
}