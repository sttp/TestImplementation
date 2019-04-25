using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTP.Net
{
    [CommandName("CtpClientCerts")]
    public class CtpClientCert
        : CommandObject<CtpClientCert>
    {
        [CommandField()]
        public string CertificateName { get; set; }

        [CommandField()]
        public string CertificateDirectory { get; set; }

        [CommandField()]
        public string MappedAccount { get; set; }

        [CommandField()]
        public List<IpAndMask> AllowedRemoteIPs { get; set; }

        public CtpClientCert()
        {
            AllowedRemoteIPs = new List<IpAndMask>();
        }

        public static explicit operator CtpClientCert(CtpCommand obj)
        {
            return FromCommand(obj);
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

                if (string.IsNullOrWhiteSpace(CertificateDirectory))
                {
                    sb.Append("Missing Certificates; ");
                }
                else
                {
                    sb.Append("Certs: " + Path.GetDirectoryName(CertificateDirectory) + "; ");
                }
                return sb.ToString();
            }
        }
    }
}