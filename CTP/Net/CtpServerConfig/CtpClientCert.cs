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
        public List<string> CertificatePaths { get; set; }

        [CommandField()]
        public string MappedAccount { get; set; }

        [CommandField()]
        public List<IpAndMask> AllowedRemoteIPs { get; set; }

        public CtpClientCert()
        {
            AllowedRemoteIPs = new List<IpAndMask>();
            CertificatePaths = new List<string>();
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