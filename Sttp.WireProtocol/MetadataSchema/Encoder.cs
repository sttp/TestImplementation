using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.MetadataSchema
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.MetadataSchema;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void GetMetadataSchemaResponse(MetadataSchemaDefinition schema)
        {
            BeginCommand();
            Stream.Write(schema);
            EndCommand();
        }
     

    }
}
