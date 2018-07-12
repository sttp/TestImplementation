
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
        /// This is a CtpDocument that corresponds to <see cref="Ticket"/>. 
        /// </summary>
        [DocumentField()] public byte[] Ticket { get; private set; }

        public AuthResume(byte[] ticket)
        {
            Ticket = ticket;
        }

        private AuthResume()
        {

        }

        public static explicit operator AuthResume(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}