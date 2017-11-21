using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol
{
    public class CommandMetadataSchema
    {
        public MetadataSchemaDefinition Schema;

        public void Load(PayloadReader reader)
        {
            Schema = new MetadataSchemaDefinition(reader);
        }
    }
}