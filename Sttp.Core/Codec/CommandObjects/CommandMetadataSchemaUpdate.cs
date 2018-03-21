using System;
using System.Collections.Generic;
using System.Text;

namespace Sttp.Codec
{
    public class CommandMetadataSchemaUpdate : CommandBase
    {
        public readonly Guid SchemaVersion;
        public readonly long SequenceNumber;
        public readonly List<MetadataSchemaTableUpdate> Tables;

        public CommandMetadataSchemaUpdate(Guid schemaVersion, long sequenceNumber, List<MetadataSchemaTableUpdate> tables)
            : base("MetadataSchemaUpdate")
        {
            SchemaVersion = schemaVersion;
            SequenceNumber = sequenceNumber;
            Tables = new List<MetadataSchemaTableUpdate>(tables);
        }

        public CommandMetadataSchemaUpdate(SttpMarkupReader reader)
            : base("MetadataSchemaUpdate")
        {
            var element = reader.ReadEntireElement();

            SchemaVersion = (Guid)element.GetValue("SchemaVersion");
            SequenceNumber = (long)element.GetValue("SequenceNumber");

            foreach (var query in element.GetElement("Tables").ChildElements)
            {
                Tables.Add(new MetadataSchemaTableUpdate(query));
            }
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("SchemaVersion", SchemaVersion);
            writer.WriteValue("SequenceNumber", SequenceNumber);
            using (writer.StartElement("Tables"))
            {
                foreach (var q in Tables)
                {
                    using (writer.StartElement("Table"))
                    {
                        q.Save(writer);
                    }
                }
            }
        }
    }
}