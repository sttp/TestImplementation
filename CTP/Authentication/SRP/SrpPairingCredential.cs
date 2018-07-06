using System;

namespace CTP.SRP
{
    public class SrpPairingCredential<T>
    {
        public readonly SrpVerifier Verifier;

        public readonly DateTime ExpireTime;

        public readonly string AssignedCredentialName;

        public readonly T Token;

        /// <summary>
        /// Creates a user credential from the provided data.
        /// </summary>
        /// <param name="pairingName"></param>
        /// <param name="paringPin"></param>
        /// <param name="token"></param>
        /// <param name="expireTime"></param>
        /// <param name="assignedCredentialName"></param>
        /// <returns></returns>
        public SrpPairingCredential(string pairingName, string paringPin, DateTime expireTime, string assignedCredentialName, T token)
        {
            Verifier = new SrpVerifier(pairingName, paringPin);
            ExpireTime = expireTime;
            AssignedCredentialName = assignedCredentialName;
            Token = token;
        }
    }
}