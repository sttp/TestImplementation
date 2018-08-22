using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GSF;

namespace CTP.Net
{
    [DocumentName("CtpInstalledCertificates")]
    public class CtpInstalledCertificates
        : DocumentObject<CtpInstalledCertificates>
    {
        [DocumentField()]
        public bool IsEnabled { get; set; }
        [DocumentField()]
        public string Name { get; set; }
        [DocumentField()]
        public bool EnableSSL { get; set; }
        [DocumentField()]
        public string CertificatePath { get; set; }
        [DocumentField()]
        public List<IpAndMask> RemoteIPs { get; set; }

        public CtpInstalledCertificates()
        {
            RemoteIPs = new List<IpAndMask>();
        }

        public static explicit operator CtpInstalledCertificates(CtpDocument obj)
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
                if (!string.IsNullOrWhiteSpace(Name))
                    sb.Append("Name: " + Name + "; ");
                if (!EnableSSL)
                {
                    sb.Append("SSL: OFF; ");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(CertificatePath))
                    {
                        sb.Append("Missing Certificate; ");
                    }
                    else
                    {
                        sb.Append("Cert: " + Path.GetFileName(CertificatePath) + "; ");
                    }
                }
                if (RemoteIPs != null && RemoteIPs.Count > 0)
                {
                    sb.Append($"Remote IPs: {string.Join(", ", RemoteIPs.Select(x=>x.DisplayMember))}; ");
                }
                return sb.ToString();
            }
        }
    }
}