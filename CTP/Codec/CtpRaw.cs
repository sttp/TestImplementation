using CTP;

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

        public CtpRaw(byte[] payload, byte channel)
        {
            Payload = payload;
            Channel = channel;
        }

        //Exists to support CtpSerializable
        private CtpRaw()
        {

        }

        public static explicit operator CtpRaw(CtpCommand obj)
        {
            return FromCommand(obj);
        }


    }
}