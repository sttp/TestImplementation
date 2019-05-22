
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CTP
{
    [CommandName("CertificateProof")]
    public class CertificateProof
        : CommandObject<CertificateProof>
    {
        [CommandField()]
        public byte[] EphemeralCertificate { get; private set; }

        public CertificateProof(byte[] ephemeralCertificate)
        {
            EphemeralCertificate = ephemeralCertificate;
        }

        private CertificateProof()
        {
        }

        public static explicit operator CertificateProof(CtpCommand obj)
        {
            return FromCommand(obj);
        }
       
    }
}