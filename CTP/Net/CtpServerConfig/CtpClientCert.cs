using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
        public string MappedAccount { get; set; }

        [DocumentField()]
        public List<IpAndMask> AllowedRemoteIPs { get; set; }

        public CtpClientCert()
        {
            AllowedRemoteIPs = new List<IpAndMask>();
        }

        public static explicit operator CtpClientCert(CtpDocument obj)
        {
            return FromDocument(obj);
        }

        public string DisplayMember
        {
            get
            {
                var sb = new StringBuilder();
                if (!IsEnabled)
                    sb.Append("(Disabled) ");

                if (!string.IsNullOrWhiteSpace(CertificateName))
                    sb.Append("Name: " + CertificateName + " ");

                if (!string.IsNullOrWhiteSpace(MappedAccount))
                    sb.Append("=> " + MappedAccount + "; ");

                if (string.IsNullOrWhiteSpace(CertificatePath))
                {
                    sb.Append("Missing Certificate; ");
                }
                else
                {
                    sb.Append("Cert: " + Path.GetFileName(CertificatePath) + "; ");
                }
                return sb.ToString();
            }
        }
    }
}