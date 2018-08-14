using System;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;

namespace CTP.SRP
{
    /// <summary>
    /// Provides the client poof and finishes the key exchange.
    /// </summary>
    [DocumentName("CertExchangeClientProof")]
    public class CertExchangeClientProof
        : DocumentObject<CertExchangeClientProof>
    {
        /// <summary>
        /// Client proof is HMAC(K,'Client Proof')
        /// </summary>
        [DocumentField()] public byte[] ClientProof { get; private set; }

        public CertExchangeClientProof(byte[] clientProof)
        {
            ClientProof = clientProof;
        }

        private CertExchangeClientProof()
        {

        }

        public static explicit operator CertExchangeClientProof(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}