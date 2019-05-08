
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CTP
{
    /// <summary>
    /// Client requests authentication using session tickets. These tickets can be granted by a ticketing authority,
    /// or by possessing the private key of an approved certificate.
    /// </summary>
    [CommandName("Auth")]
    public class Auth
        : CommandObject<Auth>
    {
        /// <summary>
        /// This field is a <see cref="CtpCommand"/> defined by <see cref="CommandObject"/> <see cref="Ticket"/>.
        /// </summary>
        [CommandField()]
        public byte[] Ticket { get; private set; }

        /// <summary>
        /// The thumbprint of the certificate that was used to sign the authorizationTicket.
        /// Note: this certificate must pre-exist on the server.
        /// </summary>
        [CommandField()]
        public string CertificateThumbprint;

        /// <summary>
        /// The signature 
        /// </summary>
        [CommandField()]
        public byte[] Signature;

        public Auth(byte[] authorizationTicket, string certificateThumbprint, byte[] signature)
        {
            Ticket = authorizationTicket;
            CertificateThumbprint = certificateThumbprint;
            Signature = signature;
        }

        public Auth(byte[] authorizationTicket, X509Certificate2 certificate)
        {
            Ticket = authorizationTicket;
            CertificateThumbprint = certificate.Thumbprint;

            using (var ecdsa = certificate.GetECDsaPrivateKey())
            {
                if (ecdsa != null)
                {
                    Signature = ecdsa.SignData(Ticket, HashAlgorithmName.SHA256);
                    return;
                }
            }
            using (var rsa = certificate.GetRSAPrivateKey())
            {
                if (rsa != null)
                {
                    Signature = rsa.SignData(Ticket, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    return;
                }
            }
            throw new Exception("Certificate does not have a private key.");
        }

        public bool ValidateSignature(X509Certificate2 certificate)
        {
            using (var ecdsa = certificate.GetECDsaPublicKey())
            {
                if (ecdsa != null)
                    return ecdsa.VerifyData(Ticket, Signature, HashAlgorithmName.SHA256);
            }
            using (var rsa = certificate.GetRSAPublicKey())
            {
                if (rsa != null)
                    return rsa.VerifyData(Ticket, Signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            throw new Exception("Certificate signing algorithm is invalid.");
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