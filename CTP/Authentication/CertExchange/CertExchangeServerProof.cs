using System;
using System.Collections.Generic;
using CTP.Authentication;
using CTP.Net;

namespace CTP.SRP
{
    /// <summary>
    /// Proves to the client that the server is not an imposer.
    /// </summary>
    [DocumentName("CertExchangeServerProof")]
    public class CertExchangeServerProof
        : DocumentObject<CertExchangeServerProof>
    {
        /// <summary>
        /// The server proof is HMAC(K,'Server Proof')
        /// </summary>
        [DocumentField()] public byte[] ServerProof { get; private set; }

        public CertExchangeServerProof(byte[] serverProof)
        {
            ServerProof = serverProof;
        }

        private CertExchangeServerProof()
        {

        }

        public static explicit operator CertExchangeServerProof(CtpDocument obj)
        {
            return FromDocument(obj);
        }
      
    }
}