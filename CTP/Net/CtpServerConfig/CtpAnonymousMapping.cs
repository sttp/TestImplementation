using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GSF;

namespace CTP.Net
{
    [CommandName("CtpAnonymousMapping")]
    public class CtpAnonymousMapping 
        : CommandObject<CtpAnonymousMapping>
    {
        [CommandField()]
        public string Name { get; set; }

        [CommandField()]
        public string MappedAccount { get; set; }

        [CommandField()]
        public IpAndMask TrustedIPs { get; set; }

        public CtpAnonymousMapping()
        {
            TrustedIPs = new IpAndMask() { IpAddress = "255.255.255.255", MaskBits = 32 };
        }

        public static explicit operator CtpAnonymousMapping(CtpCommand obj)
        {
            return FromCommand(obj);
        }

        public string DisplayMember
        {
            get
            {
                var sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(Name))
                    sb.Append(Name + ": ");

                if (TrustedIPs != null)
                    sb.Append(TrustedIPs.DisplayMember + " => ");

                if (!string.IsNullOrWhiteSpace(MappedAccount))
                    sb.Append(MappedAccount);
             
                return sb.ToString();
            }
        }
    }
}