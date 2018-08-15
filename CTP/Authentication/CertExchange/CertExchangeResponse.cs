using System;
using System.Numerics;
using System.Security;

namespace CTP.SRP
{
    /// <summary>
    /// The response for a <see cref="CertExchange"/>
    /// </summary>
    [DocumentName("CertExchangeResponse")]
    public class CertExchangeResponse
        : DocumentObject<CertExchangeResponse>
    {
        /// <summary>
        /// Public-B for the SRP key exchange algorithm. B = k*v + g^b
        /// </summary>
        [DocumentField()] public byte[] PublicB { get; private set; }

        public CertExchangeResponse(byte[] publicB)
        {
            PublicB = publicB;
        }

        private CertExchangeResponse()
        {

        }

        public static explicit operator CertExchangeResponse(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}