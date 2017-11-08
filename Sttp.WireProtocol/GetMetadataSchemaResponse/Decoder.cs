using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.GetMetadataSchemaResponse
{
    public class Decoder
    {
        public MetadataSchema Schema;

        public CommandCode CommandCode => CommandCode.GetMetadataSchemaResponse;

        public void Fill(PacketReader reader)
        {
            Schema = new MetadataSchema(reader);
        }
    }

    
}
