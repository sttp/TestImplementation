using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.Metadata
{
    public class CmdSchema : ICmd
    {
        public SubCommand SubCommand => SubCommand.Schema;

        public MetadataSchemaDefinition Schema;

        public void Load(PacketReader reader)
        {
            Schema = new MetadataSchemaDefinition(reader);
        }
    }
}