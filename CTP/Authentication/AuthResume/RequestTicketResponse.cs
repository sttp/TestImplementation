using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP.Net;

namespace CTP.Authentication
{
    /// <summary>
    /// Requests a ticket that can be used to resume a session.
    /// </summary>
    [DocumentName("RequestTicketResponse")]
    public class RequestTicketResponse
        : DocumentObject<RequestTicketResponse>
    {
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
        /// The key that can be used to sign session secrets. 
        /// </summary>
        [DocumentField()] public byte[] TicketSigningKey { get; private set; }

        /// <summary>
        /// String names for each of the roles this credential possesses.
        /// </summary>
        [DocumentField()] public string[] Roles { get; private set; }

        public RequestTicketResponse(uint masterSecretID, uint remainingSeconds, uint expireTimeMinutes, uint credentialNameID, byte[] ticketSigningKey, string[] roles)
        {
            MasterSecretID = masterSecretID;
            RemainingSeconds = remainingSeconds;
            ExpireTimeMinutes = expireTimeMinutes;
            CredentialNameID = credentialNameID;
            TicketSigningKey = ticketSigningKey;
            Roles = roles;
        }

        private RequestTicketResponse()
        {

        }
        
        public static explicit operator RequestTicketResponse(CtpDocument obj)
        {
            return FromDocument(obj);
        }


        public ClientResumeTicket CreateResumeTicket(string loginName, string[] selectedRoles)
        {
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
            t.Sign(TicketSigningKey);
            return new ClientResumeTicket(t.ToArray(), Security.ComputeHMAC(TicketSigningKey, t.Signature));
        }

    }
}
}
