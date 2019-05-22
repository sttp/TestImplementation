using System.Collections.Generic;
using System.Security.Cryptography;

namespace CTP
{
    [CommandName("ClientDone")]
    public class ClientDone
        : CommandObject<ClientDone>
    {
        /// <summary>
        /// The requested Service Provider Name
        /// </summary>
        [CommandField()]
        public string SPN { get; private set; }

        /// <summary>
        /// A string that identifies the user of this ticket.
        /// </summary>
        [CommandField()]
        public string LoginName { get; private set; }

        /// <summary>
        /// The list of roles granted to this certificate.
        /// </summary>
        [CommandField()]
        public List<string> GrantedRoles { get; private set; }

        /// <summary>
        /// The list of roles granted to this certificate.
        /// </summary>
        [CommandField()]
        public List<string> DeniedRoles { get; private set; }

        public ClientDone(string spn, string loginName, List<string> grantedRoles, List<string> deniedRoles)
        {
            SPN = spn;
            LoginName = loginName;
            GrantedRoles = grantedRoles;
            DeniedRoles = deniedRoles;
        }

        private ClientDone()
        {
        }

        public static explicit operator ClientDone(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}