using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTP
{
    public class CommandRaw
    {
        public readonly int ChannelID;
        public readonly byte[] Payload;

        public CommandRaw(int channelID, byte[] payload)
        {
            ChannelID = channelID;
            Payload = payload;
        }
    }
}
