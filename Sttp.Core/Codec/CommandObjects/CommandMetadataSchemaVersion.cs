using System;
using System.Collections.Generic;
using System.Text;

namespace Sttp.Codec
{
    public class CommandMetadataSchemaVersion : CommandBase
    {
        public readonly Guid SchemaVersion;
        public readonly long SequenceNumber;

        public CommandMetadataSchemaVersion(Guid schemaVersion, long sequenceNumber)
            : base("MetadataSchemaVersion")
        {
            SchemaVersion = schemaVersion;
            SequenceNumber = sequenceNumber;
        }

        public CommandMetadataSchemaVersion(SttpMarkupReader reader)
            : base("MetadataSchemaVersion")
        {
            var element = reader.ReadEntireElement();

            SchemaVersion = (Guid)element.GetValue("SchemaVersion");
            SequenceNumber = (long)element.GetValue("SequenceNumber");

            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("SchemaVersion", SchemaVersion);
            writer.WriteValue("SequenceNumber", SequenceNumber);
        }
    }
}