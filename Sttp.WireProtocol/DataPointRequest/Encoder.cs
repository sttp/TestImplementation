using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.DataPointRequest
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.DataPointRequest;

        public Encoder(CommandEncoder commandEncoder, SessionDetails sessionDetails)
            : base(commandEncoder, sessionDetails)
        {

        }
        
        public void DataPointRequest(SttpNamedSet options, SttpDataPointID[] dataPoints)
        {
            Stream.Write(CommandCode.DataPointRequest);
            Stream.Write(options);
            Stream.Write(dataPoints);
        }


    }
}
