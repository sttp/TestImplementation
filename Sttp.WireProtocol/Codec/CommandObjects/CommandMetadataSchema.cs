using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandMetadataSchema
    {
        public readonly MetadataSchemaDefinition Schema;

        public CommandMetadataSchema(PayloadReader reader)
        {
            Schema = new MetadataSchemaDefinition(reader);
        }
    }
}