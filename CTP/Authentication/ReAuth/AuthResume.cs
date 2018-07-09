using System;

namespace CTP.SRP
{
    /// <summary>
    /// Requests resuming an authentication session that was started in another session.
    /// </summary>
    [DocumentName("AuthResume")]
    public class AuthResume
        : DocumentObject<AuthResume>
    {
        /// <summary>
        /// A ticket that must be presented to the server in order to resume a session.
        /// </summary>
        [DocumentField()] public byte[] SessionTicket { get; private set; }

        /// <summary>
        /// The start time of the ticket or time granted by an authentication server.
        /// </summary>
        [DocumentField()] public DateTime ValidAfter { get; private set; }

        /// <summary>
        /// The end time of the ticket or time granted by an authentication server.
        /// </summary>
        [DocumentField()] public DateTime ValidBefore { get; private set; }

        /// <summary>
        /// The login name to associate with this session.
        /// </summary>
        [DocumentField()] public string LoginName { get; private set; }

        /// <summary>
        /// The available roles for this ticket.
        /// </summary>
        [DocumentField()] public string[] Roles { get; private set; }

        /// <summary>
        /// Entropy to assign to the ticket. This salt supports an authentication server's 
        /// ability to create derived session that all have different challenge/response secrets.
        /// </summary>
        [DocumentField()] public byte[] TicketSalt { get; private set; }

        /// <summary>
        /// Using derived key 3, the proof is the HMAC of the ticket contents.
        /// </summary>
        [DocumentField()] public byte[] TicketHMAC { get; private set; }

        private AuthResume()
        {

        }

        public static explicit operator AuthResume(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}