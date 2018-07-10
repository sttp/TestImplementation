using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CTP.Net;
using CTP.SRP;
using GSF.Units;

namespace CTP.Authentication
{
    [DocumentName("SessionTicket")]
    public class SessionTicket
        : DocumentObject<SessionTicket>
    {
        /// <summary>
        /// The ticket information supplied by the server.
        /// </summary>
        [DocumentField()] public byte[] Ticket { get; private set; }

        /// <summary>
        /// The name of the credential associated with this ticket.
        /// </summary>
        [DocumentField()] public string CredentialName { get; private set; }

        /// <summary>
        /// Salt information for this ticket so it's identity is unique
        /// </summary>
        [DocumentField()] public byte[] TicketSalt { get; private set; }

        /// <summary>
        /// The approved start time of the ticket. 
        /// </summary>
        [DocumentField()] public DateTime ValidAfter { get; private set; }

        /// <summary>
        /// The approved end time of the ticket. 
        /// </summary>
        [DocumentField()] public DateTime ValidBefore { get; private set; }

        /// <summary>
        /// The available roles for this ticket.
        /// </summary>
        [DocumentField()] public string[] Roles { get; private set; }

        public SessionTicket(byte[] ticket, string credentialName, byte[] ticketSalt, DateTime validAfter, DateTime validBefore, string[] roles)
        {
            Ticket = ticket;
            CredentialName = credentialName;
            TicketSalt = ticketSalt;
            ValidAfter = validAfter;
            ValidBefore = validBefore;
            Roles = roles;
        }

        public ClientResumeTicket CreateClientTicket(byte[] ticketSigningBytes, byte[] challengeResponseKey)
        {
            AuthResume resume = new AuthResume(ToDocument().ToArray(), null);
            resume.Sign(ticketSigningBytes);

            using (var hmac = new HMACSHA256(challengeResponseKey))
            {
                return new ClientResumeTicket(resume.SessionTicket, resume.SessionTicketHMAC, hmac.ComputeHash(TicketSalt, 0, TicketSalt.Length));
            }
        }

        private SessionTicket()
        {

        }

        public static explicit operator SessionTicket(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
