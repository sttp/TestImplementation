using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandDataPointRequest
    {
        public CommandCode CommandCode => CommandCode.DataPointRequest;

        public readonly SttpNamedSet Options;
        public readonly SttpDataPointID[] DataPoints;

        public CommandDataPointRequest(PayloadReader reader)
        {
            Options = reader.Read<SttpNamedSet>();
            DataPoints = reader.ReadArray<SttpDataPointID>();
        }
    }
}
