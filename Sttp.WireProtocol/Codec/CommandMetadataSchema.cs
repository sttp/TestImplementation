using System;
using System.Collections.Generic;

namespace Sttp.Codec
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