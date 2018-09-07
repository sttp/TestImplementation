using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GSF;

namespace CTP.Net
{
    [DocumentName("CtpAnonymousMapping")]
    public class CtpAnonymousMapping
        : DocumentObject<CtpAnonymousMapping>
    {
        [DocumentField()]
        public string Name { get; set; }

        [DocumentField()]
        public string MappedAccount { get; set; }

        [DocumentField()]
        public IpAndMask AccessList { get; set; }

        public CtpAnonymousMapping()
        {
            AccessList = new IpAndMask() { IpAddress = "255.255.255.255", MaskBits = 32 };
        }

        public static explicit operator CtpAnonymousMapping(CtpDocument obj)
        {
            return FromDocument(obj);
        }

        public string DisplayMember
        {
            get
            {
                var sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(Name))
                    sb.Append(Name + ": ");

                if (AccessList != null)
                    sb.Append(AccessList.DisplayMember + " => ");

                if (!string.IsNullOrWhiteSpace(MappedAccount))
                    sb.Append(MappedAccount);
             
                return sb.ToString();
            }
        }
    }
}