using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CTP.Net
{
    [DocumentName("CtpAccount")]
    public class CtpAccount
        : DocumentObject<CtpAccount>
    {
        [DocumentField()]
        public bool IsEnabled { get; set; }

        [DocumentField()]
        public string Name { get; set; }

        [DocumentField()]
        public string Description { get; set; }

        /// <summary>
        /// All of the roles granted with this account.
        /// </summary>
        [DocumentField()]
        public List<string> Roles { get; set; }

        [DocumentField()]
        public List<IpAndMask> AllowedRemoteIPs { get; set; }

        [DocumentField()]
        public List<CtpClientCert> ClientCerts { get; set; }

        public CtpAccount()
        {
            AllowedRemoteIPs = new List<IpAndMask>();
            ClientCerts = new List<CtpClientCert>();
        }

        public static explicit operator CtpAccount(CtpDocument obj)
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
                sb.Append($"Clients: {ClientCerts?.Count ?? 0}; ");
                if (AllowedRemoteIPs != null && AllowedRemoteIPs.Count > 0)
                {
                    sb.Append($"Allowed IPs: {string.Join(", ", AllowedRemoteIPs.Select(x => x.DisplayMember))}; ");
                }

                if (Roles != null)
                {
                    sb.Append("Roles: " + string.Join(", ", Roles) + "; ");
                }

                return sb.ToString();
            }
        }
    }
}