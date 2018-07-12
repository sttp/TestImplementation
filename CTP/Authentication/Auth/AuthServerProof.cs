using System;
using System.Collections.Generic;
using CTP.Authentication;
using CTP.Net;

namespace CTP.SRP
{
    /// <summary>
    /// Proves to the client proof that the server is not an imposer, and specifies a ticket that can be used to resume this session at a later date.
    /// </summary>
    [DocumentName("AuthServerProof")]
    public class AuthServerProof
        : DocumentObject<AuthServerProof>
    {
        /// <summary>
        /// The server proof is HMAC(K,'Server Proof')
        /// </summary>
        [DocumentField()] public byte[] ServerProof { get; private set; }

        /// <summary>
        /// Identifies the master secret for the ticket.
        /// </summary>
        [DocumentField()] public uint MasterSecretID { get; private set; }

        /// <summary>
        /// Gets the number of seconds that remain before this ticket expires. 
        /// This field is used to shorten the expiration time if desired. This allows for
        /// clocks to not be tightly time synchronized.
        /// </summary>
        [DocumentField()] public uint RemainingSeconds { get; private set; }

        /// <summary>
        /// Gets the number of minutes this ticket is valid.
        /// </summary>
        [DocumentField()] public uint ExpireTimeMinutes { get; private set; }

        /// <summary>
        /// A sequence number that identifies which credential name created this ticket.
        /// </summary>
        [DocumentField()] public uint CredentialNameID { get; private set; }

        /// <summary>
        /// The key that can be used to sign session secrets. This field is XOR'd with 
        /// HMAC(K,'Ticket Key')
        /// </summary>
        [DocumentField()] public byte[] EncryptedTicketSigningKey { get; private set; }

        /// <summary>
        /// String names for each of the roles this credential possesses.
        /// </summary>
        [DocumentField()] public string[] Roles { get; private set; }

        public AuthServerProof(byte[] serverProof, uint masterSecretID, uint remainingSeconds, uint expireTimeMinutes, uint credentialNameID, byte[] encryptedTicketSigningKey, string[] roles)
        {
            ServerProof = serverProof;
            MasterSecretID = masterSecretID;
            RemainingSeconds = remainingSeconds;
            ExpireTimeMinutes = expireTimeMinutes;
            CredentialNameID = credentialNameID;
            EncryptedTicketSigningKey = encryptedTicketSigningKey;
            Roles = roles;
        }

        private AuthServerProof()
        {

        }

        public static explicit operator AuthServerProof(CtpDocument obj)
        {
            return FromDocument(obj);
        }

        public ClientResumeTicket CreateResumeTicket(byte[] srpK, string loginName, string[] selectedRoles)
        {
            byte[] ticketKey = Security.ComputeHMAC(srpK, "Ticket Key");
            byte[] signingKey = Security.XOR(ticketKey, EncryptedTicketSigningKey);
            List<uint> roles = new List<uint>();
            if (selectedRoles != null)
            {
                foreach (var role in selectedRoles)
                {
                    int id = Array.IndexOf(Roles, role);
                    if (id < 0)
                        throw new ArgumentException("Role is not granted by this ticket", nameof(selectedRoles));
                    roles.Add((uint)id);
                }
            }

            var t = new Ticket(MasterSecretID, RemainingSeconds, ExpireTimeMinutes, CredentialNameID, roles, loginName, null);
            t.Sign(signingKey);
            return new ClientResumeTicket(t.ToArray(), Security.ComputeHMAC(signingKey, t.Signature));
        }

    }
}