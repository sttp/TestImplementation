using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.SendDataPointsCustom
{
    public class Decoder
    {
        public List<SttpDataPointID> Points;

        public CommandCode CommandCode => CommandCode.SendDataPointsCustom;

        public byte EncodingMethod;
        public byte[] Data;

        public void Fill(PacketReader reader)
        {
            EncodingMethod = reader.ReadByte();
            Data = reader.ReadBytes();
        }
    }
}
