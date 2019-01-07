
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CTP
{
    /// <summary>
    /// Requests resuming an authentication session that was started in another session.
    /// </summary>
    [CommandName("Auth")]
    public class Auth
        : CommandObject<Auth>
    {
        /// <summary>
        /// This field is a CTPDocument of type <see cref="Ticket"/>.
        /// </summary>
        [CommandField()]
        public CtpCommand Ticket { get; private set; }

        [CommandField()]
        public string AuthorizationCertificate;

        [CommandField()]
        public byte[] Signature;

        public Auth(Ticket ticket, X509Certificate2 certificate)
        {
            Ticket = ticket.ToCommand();
            AuthorizationCertificate = certificate.Thumbprint;
            using (var rsa = certificate.GetRSAPrivateKey())
            {
                Signature = rsa.SignData(Ticket.ToArray(), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        public bool ValidateSignature(X509Certificate2 certificate)
        {
            using (var rsa = certificate.GetRSAPublicKey())
            {
                return rsa.VerifyData(Ticket.ToArray(), Signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        private Auth()
        {

        }

        public static explicit operator Auth(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}