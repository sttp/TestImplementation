using System;
using System.Collections.Generic;
using System.Text;

namespace Sttp.Codec
{
    public class CommandMetadataSchemaUpdate : CommandBase
    {
        public readonly Guid SchemaVersion;
        public readonly long Revision;
        public readonly long UpdatedFromVersion;
        public readonly List<MetadataSchemaTableUpdate> Tables;

        public CommandMetadataSchemaUpdate(Guid schemaVersion, long revision, long updatedFromVersion, List<MetadataSchemaTableUpdate> tables)
            : base("MetadataSchemaUpdate")
        {
            SchemaVersion = schemaVersion;
            Revision = revision;
            UpdatedFromVersion = updatedFromVersion;
            Tables = new List<MetadataSchemaTableUpdate>(tables);
        }

        public CommandMetadataSchemaUpdate(SttpMarkupReader reader)
            : base("MetadataSchemaUpdate")
        {
            var element = reader.ReadEntireElement();
            if (element.ElementName != "MetadataSchemaUpdate")
                throw new Exception("Invalid command");

            SchemaVersion = (Guid)element.GetValue("SchemaVersion");
            Revision = (long)element.GetValue("Revision");
            UpdatedFromVersion = (long)element.GetValue("UpdatedFromVersion");

            foreach (var query in element.GetElement("Tables").ChildElements)
            {
                Tables.Add(new MetadataSchemaTableUpdate(query));
            }
            element.ErrorIfNotHandled();
        }

        public override CommandBase Load(SttpMarkupReader reader)
        {
            return new CommandMetadataSchema(reader);
        }

        public override void Save(SttpMarkupWriter writer)
        {
            using (writer.StartElement(CommandName))
            {
                writer.WriteValue("SchemaVersion", SchemaVersion);
                writer.WriteValue("Revision", Revision);
                writer.WriteValue("UpdatedFromVersion", UpdatedFromVersion);
                using (writer.StartElement("Tables"))
                {
                    foreach (var q in Tables)
                    {
                        q.Save(writer);
                    }
                }

            }
        }
    }
}