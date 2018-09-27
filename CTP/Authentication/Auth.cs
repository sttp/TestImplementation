
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CTP
{
    /// <summary>
    /// Requests resuming an authentication session that was started in another session.
    /// </summary>
    [DocumentName("Auth")]
    public class Auth
        : DocumentObject<Auth>
    {
        /// <summary>
        /// This field is a CTPDocument of type <see cref="Ticket"/>.
        /// </summary>
        [DocumentField()]
        public CtpDocument Ticket { get; private set; }

        [DocumentField()]
        public string AuthorizationCertificate;

        [DocumentField()]
        public byte[] Signature;

        public Auth(Ticket ticket, X509Certificate2 certificate)
        {
            Ticket = ticket.ToDocument();
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

        public static explicit operator Auth(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}