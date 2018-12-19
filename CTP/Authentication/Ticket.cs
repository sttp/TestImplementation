using System;
using System.Collections.Generic;

namespace CTP
{
    [CommandName("Ticket")]
    public class Ticket
        : CommandObject<Ticket>
    {
        /// <summary>
        /// Gets the UTC time this ticket is valid from.
        /// </summary>
        [CommandField()]
        public DateTime ValidFrom { get; private set; }

        /// <summary>
        /// Gets the UTC time this ticket is valid until.
        /// </summary>
        [CommandField()]
        public DateTime ValidTo { get; private set; }

        /// <summary>
        /// A string that identifies the user of this ticket.
        /// </summary>
        [CommandField()]
        public string LoginName { get; private set; }

        /// <summary>
        /// A string that identifies the source IP address for this ticket. This prevents ticket sharing.
        /// </summary>
        [CommandField()]
        public string ValidFor { get; private set; }

        /// <summary>
        /// The list of roles granted by this ticket.
        /// </summary>
        [CommandField()]
        public List<string> Roles { get; private set; }

        /// <summary>
        /// The list of approved certificates that the remote resource may use. 
        /// This is the SHA-256 hash of the public key.
        /// </summary>
        [CommandField()]
        public List<string> ApprovedClientCertificates { get; private set; }

        private Ticket()
        {

        }

        public static explicit operator Ticket(CtpCommand obj)
        {
            return FromDocument(obj);
        }

        public Ticket(DateTime validFrom, DateTime validTo, string loginName, string validFor, List<string> roles, List<string> approvedClientCertificates)
        {
            ValidFrom = validFrom;
            ValidTo = validTo;
            LoginName = loginName;
            ValidFor = validFor;
            Roles = roles;
            ApprovedClientCertificates = approvedClientCertificates;
        }
    }
}
