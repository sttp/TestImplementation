using System.Collections.Generic;

namespace CTP.Net
{
    [DocumentName("CtpClientCerts")]
    public class CtpClientCert
        : DocumentObject<CtpClientCert>
    {
        [DocumentField()]
        public bool IsEnabled { get; set; }

        [DocumentField()]
        public bool SupportsTickets { get; set; }

        [DocumentField()]
        public string CertificateName { get; set; }
        /// <summary>
        /// Note: The pairing PIN is only valid if there is a file in this path.
        /// If pairing fails, the server will delete the file specified here since pairing
        /// credential cannot be used more than once. Once pairing is complete, the certificate at CertificatePath will
        /// be replaced with the paired certificate.
        /// </summary>
        [DocumentField()]
        public string PairingPinPath { get; set; }

        [DocumentField()]
        public string CertificatePath { get; set; }

        [DocumentField()]
        public List<IpAndMask> AccessList { get; set; }

        public CtpClientCert()
        {

        }

        public static explicit operator CtpClientCert(CtpDocument obj)
        {
            return FromDocument(obj);
        }

        public string DisplayMember
        {
            get
            {
                return $"{(IsEnabled ? "" : "(Disabled)")}{CertificateName}";
            }
        }
    }
}