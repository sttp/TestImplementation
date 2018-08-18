using System.Collections.Generic;

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
        public List<IpAndMask> TrustedIPs { get; set; }

        [DocumentField()]
        public List<CtpClientCert> ClientCerts { get; set; }

        public CtpAccount()
        {

        }

        public static explicit operator CtpAccount(CtpDocument obj)
        {
            return FromDocument(obj);
        }

        public string DisplayMember
        {
            get
            {
                return $"{(IsEnabled ? "" : "(Disabled)")}{Name}";
            }
        }
    }
}