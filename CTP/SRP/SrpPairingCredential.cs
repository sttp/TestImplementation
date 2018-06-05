using System;
using System.Security.Cryptography;
using System.Text;

namespace CTP.SRP
{
    public class SrpPairingCredential<T>
    {
        public readonly SrpVerifier Verifier;

        public readonly DateTime ExpireTime;

        public readonly string AssignedUserName;

        public readonly T Token;

        public readonly bool AllowCertificatePairing;

        public readonly bool AllowSessionPairing;

        /// <summary>
        /// Creates a user credential from the provided data.
        /// </summary>
        /// <param name="pairingID"></param>
        /// <param name="paringPin"></param>
        /// <param name="allowSessionPairing"></param>
        /// <param name="token"></param>
        /// <param name="expireTime"></param>
        /// <param name="assignedUserName"></param>
        /// <param name="allowCertificatePairing"></param>
        /// <returns></returns>
        public SrpPairingCredential(string pairingID, string paringPin, DateTime expireTime, string assignedUserName, bool allowCertificatePairing, bool allowSessionPairing, T token)
        {
            Verifier = new SrpVerifier(pairingID, paringPin);
            ExpireTime = expireTime;
            AssignedUserName = assignedUserName;
            AllowCertificatePairing = allowCertificatePairing;
            AllowSessionPairing = allowSessionPairing;
            Token = token;
        }
    }
}