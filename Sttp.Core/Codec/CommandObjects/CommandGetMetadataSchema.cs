using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandGetMetadataSchema : CommandBase
    {
        public readonly Guid? SchemaVersion;
        public readonly long? SequenceNumber;

        public CommandGetMetadataSchema()
            : base("GetMetadataSchema")
        {

        }

        public CommandGetMetadataSchema(Guid? schemaVersion, long? sequenceNumber)
            : this()
        {
            SchemaVersion = schemaVersion;
            SequenceNumber = sequenceNumber;
        }

        public CommandGetMetadataSchema(SttpMarkupReader reader)
            : this()
        {
            var element = reader.ReadEntireElement();

            SchemaVersion = (Guid?)element.GetValue("SchemaVersion");
            SequenceNumber = (long?)element.GetValue("SequenceNumber");
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("SchemaVersion", SchemaVersion);
            writer.WriteValue("SequenceNumber", SequenceNumber);
        }
    }
}