
using System;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public interface IServerHandshake : IDisposable
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

        ServerDone GetServerDone();

        /// <summary>
        /// Gets the certificate proof if this certificate is ephemeral.
        /// </summary>
        /// <returns></returns>
        CertificateProof GetCertificateProof();

        /// <summary>
        /// Ensures that the supplied certificate is trusted.
        /// </summary>
        /// <returns></returns>
        bool IsCertificateTrusted(CtpNetStream stream, ClientDone clientDone);

        /// <summary>
        /// Ensures that the supplied certificate is trusted in the certificate proof.
        /// </summary>
        /// <returns></returns>
        bool IsCertificateTrusted(CtpNetStream stream, CertificateProof proof);
    }
}