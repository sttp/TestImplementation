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
        /// The list of roles granted by this ticket. If blank, all roles will be assumed.
        /// </summary>
        [CommandField()]
        public List<string> Roles { get; private set; }

        /// <summary>
        /// If specified, this is the public key that the client must be using.
        /// If blank, the client does not have to supply a client side certificate.
        /// This can be used to prevent hijacking credentials.
        /// </summary>
        [CommandField()]
        public string ApprovedPublicKey { get; private set; }

        private Ticket()
        {

        }

        public static explicit operator Ticket(CtpCommand obj)
        {
            return FromCommand(obj);
        }

        public Ticket(DateTime validFrom, DateTime validTo, string loginName, List<string> roles, string approvedPublicKey)
        {
            ValidFrom = validFrom;
            ValidTo = validTo;
            LoginName = loginName;
            Roles = roles;
            ApprovedPublicKey = approvedPublicKey;
        }
    }
}
