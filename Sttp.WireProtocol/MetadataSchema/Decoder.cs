using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.MetadataSchema
{
    public class Decoder
    {
        public MetadataSchemaDefinition Schema;

        public CommandCode CommandCode => CommandCode.MetadataSchema;

        public void Fill(PacketReader reader)
        {
            Schema = new MetadataSchemaDefinition(reader);
        }
    }

    
}
