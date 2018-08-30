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
        public string CertificateName { get; set; }

        [DocumentField()]
        public List<string> CertificatePaths { get; set; }

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
                if (!string.IsNullOrWhiteSpace(CertificateName))
                    sb.Append("Name: " + CertificateName + " ");

                if (!string.IsNullOrWhiteSpace(MappedAccount))
                    sb.Append("=> " + MappedAccount + "; ");

                if (CertificatePaths == null || CertificatePaths.Count == 0)
                {
                    sb.Append("Missing Certificates; ");
                }
                else
                {
                    sb.Append("Certs: " + string.Join(", ", CertificatePaths.Select(Path.GetFileName)) + "; ");
                }
                return sb.ToString();
            }
        }
    }
}