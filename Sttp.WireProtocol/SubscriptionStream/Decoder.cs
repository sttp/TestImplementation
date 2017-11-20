﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.SubscriptionStream
{
    public class Decoder
    {
        public List<SttpDataPointID> Points;

        public CommandCode CommandCode => CommandCode.SubscriptionStream;

        public byte EncodingMethod;
        public byte[] Data;

        public void Fill(PacketReader reader)
        {
            EncodingMethod = reader.ReadByte();
            Data = reader.ReadBytes();
        }
    }
}