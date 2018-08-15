using System.Collections.Generic;

namespace CTP.Net
{
    [DocumentName("CtpServerConfig")]
    public class CtpServerConfig
        : DocumentObject<CtpServerConfig>
    {
        [DocumentField()]
        public List<CtpAccount> Accounts { get; set; }

    

        [DocumentField()]
        public List<CtpInterfaceOptions> InterfaceOptions { get; set; }

        public CtpServerConfig()
        {

        }

        public static explicit operator CtpServerConfig(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
