using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandDataPointRequest
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
