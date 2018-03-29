using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandRaw
    {
        public readonly byte ChannelID;
        public readonly byte[] Payload;

        public CommandRaw(byte channelID, byte[] payload)
        {
            ChannelID = channelID;
            Payload = payload;
        }
    }
}
