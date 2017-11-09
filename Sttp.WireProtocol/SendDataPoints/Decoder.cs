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

        public List<SttpDataPoint> DataPoints;

        public void Fill(PacketReader reader)
        {

        }
    }
}
