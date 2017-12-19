using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandGetMetadataSchema : CommandBase
    {
        public readonly Guid SchemaVersion;
        public readonly long Revision;

        public CommandGetMetadataSchema()
            : base("GetMetadataSchema")
        {
        }

        public CommandGetMetadataSchema(Guid schemaVersion, long revision)
            : this()
        {
            SchemaVersion = schemaVersion;
            Revision = revision;
        }

        public CommandGetMetadataSchema(SttpMarkupReader reader)
            : this()
        {
            var element = reader.ReadEntireElement();

            SchemaVersion = (Guid)element.GetValue("SchemaVersion");
            Revision = (long)element.GetValue("Revision");
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("SchemaVersion", SchemaVersion);
            writer.WriteValue("Revision", Revision);
        }

    }
}