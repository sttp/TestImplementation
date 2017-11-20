using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.DataPointRequest
{
    public class Decoder
    {
        public CommandCode CommandCode => CommandCode.DataPointRequest;

        public SttpNamedSet Options;
        public SttpDataPointID[] DataPoints;

        public void Fill(PayloadReader reader)
        {
            Options = reader.Read<SttpNamedSet>();
            DataPoints = reader.ReadArray<SttpDataPointID>();
        }
    }
}
