using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.SendDataPoints
{
    public class Decoder
    {
        public List<SttpDataPoint> Points;

        public CommandCode CommandCode => CommandCode.SendDataPoints;

        public byte EncodingMethod;
        public byte[] Data;

        public void Fill(PacketReader reader)
        {
            EncodingMethod = reader.ReadByte();
            Data = reader.ReadBytes();
        }
    }
}
