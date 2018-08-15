using System;

namespace CTP.Net
{
    [DocumentName("CtpPairingPin")]
    public class CtpPairingPin
        : DocumentObject<CtpPairingPin>
    {
        [DocumentField()]
        public DateTime ValidFrom { get; set; }

        [DocumentField()]
        public DateTime ValidTo { get; set; }

        [DocumentField()]
        public byte[] Verifier { get; set; }

        public CtpPairingPin()
        {

        }

        public static explicit operator CtpPairingPin(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}