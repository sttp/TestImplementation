using System.Collections.Generic;

namespace CTP.Net
{
    [DocumentName("CtpInterfaceOptions")]
    public class CtpInterfaceOptions
        : DocumentObject<CtpInterfaceOptions>
    {
        [DocumentField()]
        public bool IsEnabled { get; set; }
        [DocumentField()]
        public bool DisableSSL { get; set; }
        [DocumentField()]
        public string CertificatePath { get; set; }
        [DocumentField()]
        public List<IpAndMask> AccessList { get; set; }

        public CtpInterfaceOptions()
        {

        }

        public static explicit operator CtpInterfaceOptions(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}