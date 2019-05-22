
using System;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public interface IClientHandshake : IDisposable
    {
        /// <summary>
        /// Indicates if SSL will be used with this handshake.
        /// </summary>
        bool UseSSL { get; }

        /// <summary>
        /// Gets if the certificate will be temporary.
        /// </summary>
        bool IsEphemeralCertificate { get; }

        /// <summary>
        /// Gets the certificate that will be used to authenticate the channel.
        /// </summary>
        /// <returns></returns>
        X509Certificate2 GetCertificate();

        X509CertificateCollection GetCertificateCollection();

        ClientDone GetClientDone();

        /// <summary>
        /// Gets the certificate proof if this certificate is ephemeral.
        /// </summary>
        /// <returns></returns>
        CertificateProof GetCertificateProof();

        /// <summary>
        /// Ensures that the supplied certificate is trusted.
        /// </summary>
        /// <param name="channelCertificate"></param>
        /// <returns></returns>
        bool IsCertificateTrusted(X509Certificate channelCertificate);

        /// <summary>
        /// Ensures that the supplied certificate is trusted in the certificate proof.
        /// </summary>
        /// <param name="channelCertificate"></param>
        /// <param name="proof"></param>
        /// <returns></returns>
        bool IsCertificateTrusted(X509Certificate channelCertificate, CertificateProof proof);
    }
}