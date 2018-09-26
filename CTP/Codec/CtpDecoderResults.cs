using System;

namespace CTP
{
    public class CtpPacket
    {
        public readonly byte Channel;
        public readonly byte[] Payload;

        public CtpPacket(byte channel, byte[] payload)
        {
            Channel = channel;
            Payload = payload;
        }
    }
}