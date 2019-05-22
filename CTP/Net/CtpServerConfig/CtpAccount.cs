using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CTP.Net
{
    [CommandName("CtpAccount")]
    public class CtpAccount
        : CommandObject<CtpAccount>
    {
        [CommandField()]
        public bool IsEnabled { get; set; }

        [CommandField()]
        public string Name { get; set; }

        [CommandField()]
        public string Description { get; set; }

        /// <summary>
        /// The path to the trusted certificates on this account.
        /// </summary>
        [CommandField()]
        public string CertificateDirectory { get; set; }

        /// <summary>
        /// All of the roles granted with this account.
        /// </summary>
        [CommandField()]
        public List<string> ExplicitRoles { get; set; }

        /// <summary>
        /// All of the roles granted with this account.
        /// </summary>
        [CommandField()]
        public List<string> ImplicitRoles { get; set; }

        /// <summary>
        /// The expected remote IPs that can use this certificate if the desire is to limit its scope.
        /// </summary>
        [CommandField()]
        public List<IpAndMask> AllowedRemoteIPs { get; set; }

        public CtpAccount()
        {
            AllowedRemoteIPs = new List<IpAndMask>();
            ExplicitRoles = new List<string>();
            ImplicitRoles = new List<string>();
        }

        public static explicit operator CtpAccount(CtpCommand obj)
        {
            return FromCommand(obj);
        }

        public bool IsIPAllowed(IPAddress ip)
        {
            if (AllowedRemoteIPs == null || AllowedRemoteIPs.Count == 0)
                return true;

            var ipBytes = ip.GetAddressBytes();
            foreach (var allowed in AllowedRemoteIPs)
            {
                if (allowed.IsMatch(ipBytes))
                {
                    return true;
                }
            }
            return false;
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

                return sb.ToString();
            }
        }
    }
}