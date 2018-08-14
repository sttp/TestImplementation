using System;
using System.Collections.Generic;
using CTP.Authentication;
using CTP.Net;

namespace CTP.SRP
{
    /// <summary>
    /// Proves to the client that the server is not an imposer.
    /// </summary>
    [DocumentName("CertExchangeServerProof")]
    public class CertExchangeServerProof
        : DocumentObject<CertExchangeServerProof>
    {
        /// <summary>
        /// The server proof is HMAC(K,'Server Proof')
        /// </summary>
        [DocumentField()] public byte[] ServerProof { get; private set; }

        public CertExchangeServerProof(byte[] serverProof)
        {
            ServerProof = serverProof;
        }

        private CertExchangeServerProof()
        {

        }

        public static explicit operator CertExchangeServerProof(CtpDocument obj)
        {
            return FromDocument(obj);
        }

        //public ClientResumeTicket CreateResumeTicket(byte[] srpK, string loginName, string[] selectedRoles)
        //{
        //    byte[] ticketKey = Security.ComputeHMAC(srpK, "Ticket Key");
        //    byte[] signingKey = Security.XOR(ticketKey, EncryptedTicketSigningKey);
        //    List<uint> roles = new List<uint>();
        //    if (selectedRoles != null)
        //    {
        //        foreach (var role in selectedRoles)
        //        {
        //            int id = Array.IndexOf(Roles, role);
        //            if (id < 0)
        //                throw new ArgumentException("Role is not granted by this ticket", nameof(selectedRoles));
        //            roles.Add((uint)id);
        //        }
        //    }

        //    var t = new Ticket(MasterSecretID, RemainingSeconds, ExpireTimeMinutes, CredentialNameID, roles, loginName, null);
        //    t.Sign(signingKey);
        //    return new ClientResumeTicket(t.ToArray(), Security.ComputeHMAC(signingKey, t.Signature));
        //}

    }
}