
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CTP.Authentication;

namespace CTP.SRP
{
    /// <summary>
    /// Requests resuming an authentication session that was started in another session.
    /// </summary>
    [DocumentName("AuthTicket")]
    public class AuthTicket
        : DocumentObject<AuthTicket>
    {
        /// <summary>
        /// This field is a CTPDocument of type <see cref="Ticket"/>.
        /// </summary>
        [DocumentField()]
        public CtpDocument Ticket { get; private set; }

        [DocumentField()]
        public byte[] AuthorizationCertificate;

        [DocumentField()]
        public byte[] Signature;

        public AuthTicket(Ticket ticket, X509Certificate2 certificate)
        {
            Ticket = ticket.ToDocument();
            AuthorizationCertificate = certificate.GetPublicKey();
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

        private AuthTicket()
        {

        }

        public static explicit operator AuthTicket(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}