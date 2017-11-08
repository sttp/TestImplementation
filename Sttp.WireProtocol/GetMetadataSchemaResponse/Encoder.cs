using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.GetMetadataSchemaResponse;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void GetMetadataSchemaResponse(MetadataSchema schema)
        {
            BeginCommand();
            Stream.Write(schema);
            EndCommand();
        }
     

    }
}
