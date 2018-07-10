using System.Linq;
using System.Security.Cryptography;

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
        /// This is a CtpDocument that corresponds to <see cref="SessionTicket"/>. 
        /// </summary>
        [DocumentField()] public byte[] SessionTicket { get; private set; }

        /// <summary>
        /// The proof is the HMAC of the ticket contents. HMAC(K,'Ticket Signing')
        /// </summary>
        [DocumentField()] public byte[] SessionTicketHMAC { get; private set; }

        public AuthResume(byte[] sessionTicket, byte[] sessionTicketHMAC)
        {
            SessionTicket = sessionTicket;
            SessionTicketHMAC = sessionTicketHMAC;
        }

        private AuthResume()
        {

        }

        public bool VerifySignature(byte[] ticketSigningKey)
        {
            using (var hmac = new HMACSHA256(ticketSigningKey))
            {
                return hmac.TransformFinalBlock(SessionTicket, 0, SessionTicket.Length).SequenceEqual(SessionTicketHMAC);
            }
        }

        public void Sign(byte[] ticketSigningKey)
        {
            using (var hmac = new HMACSHA256(ticketSigningKey))
            {
                SessionTicketHMAC = hmac.TransformFinalBlock(SessionTicket, 0, SessionTicket.Length);
            }
        }

        public static explicit operator AuthResume(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}