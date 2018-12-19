using CTP;
using CTP.Serialization;

namespace CTP
{
    [CommandName("Raw")]
    public class CtpRaw
        : CommandObject<CtpRaw>
    {
        [CommandField()]
        public byte Channel;

        [CommandField()]
        public byte[] Payload;

        public CtpRaw(byte[] payload)
        {
            Payload = payload;
        }

        //Exists to support CtpSerializable
        private CtpRaw()
        {

        }

        public static explicit operator CtpRaw(CtpCommand obj)
        {
            return FromDocument(obj);
        }


    }
}