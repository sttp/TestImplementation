using CTP;
using CTP.Serialization;

namespace CTP
{
    [DocumentName("Raw")]
    public class CtpRaw
        : DocumentObject<CtpRaw>
    {
        [DocumentField()]
        public byte Channel;

        [DocumentField()]
        public byte[] Payload;

        public CtpRaw(byte[] payload)
        {
            Payload = payload;
        }

        //Exists to support CtpSerializable
        private CtpRaw()
        {

        }

        public static explicit operator CtpRaw(CtpDocument obj)
        {
            return FromDocument(obj);
        }


    }
}