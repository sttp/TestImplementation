using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.DataPointRequest
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.DataPointRequest;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
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
