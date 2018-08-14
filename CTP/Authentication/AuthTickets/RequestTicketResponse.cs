using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CTP.Net;

namespace CTP.Authentication
{
    /// <summary>
    /// Requests a ticket that can be used to resume a session.
    /// </summary>
    [DocumentName("RequestTicketResponse")]
    public class RequestTicketResponse
        : DocumentObject<RequestTicketResponse>
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

        public RequestTicketResponse(Ticket ticket, X509Certificate2 certificate)
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

        private RequestTicketResponse()
        {

        }

        public static explicit operator RequestTicketResponse(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
