using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GSF;

namespace CTP.Net
{
    [DocumentName("CtpInterfaceOptions")]
    public class CtpInterfaceOptions
        : DocumentObject<CtpInterfaceOptions>
    {
        [DocumentField()]
        public bool IsEnabled { get; set; }
        [DocumentField()]
        public string Name { get; set; }
        [DocumentField()]
        public bool DisableSSL { get; set; }
        [DocumentField()]
        public string CertificatePath { get; set; }
        [DocumentField()]
        public List<IpAndMask> AccessList { get; set; }

        public CtpInterfaceOptions()
        {
            AccessList = new List<IpAndMask>();
        }

        public static explicit operator CtpInterfaceOptions(CtpDocument obj)
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
                if (DisableSSL)
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
                if (AccessList != null && AccessList.Count > 0)
                {
                    sb.Append($"Remote IPs: {string.Join(", ", AccessList.Select(x=>x.DisplayMember))}; ");
                }
                return sb.ToString();
            }
        }
    }
}