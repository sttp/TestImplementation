using System;
using System.Collections.Generic;
using System.Text;

namespace Sttp.Codec
{
    public class CommandMetadataSchema : CommandBase
    {
        public readonly Guid SchemaVersion;
        public readonly long Revision;
        public readonly List<MetadataSchemaTables> Tables;

        public CommandMetadataSchema(Guid schemaVersion, long revision, List<MetadataSchemaTables> tables)
            : base("MetadataSchema")
        {
            SchemaVersion = schemaVersion;
            Revision = revision;
            Tables = new List<MetadataSchemaTables>(tables);
        }

        public CommandMetadataSchema(SttpMarkupReader reader)
            : base("MetadataSchema")
        {
            Tables = new List<MetadataSchemaTables>();
            var element = reader.ReadEntireElement();

            SchemaVersion = (Guid)element.GetValue("SchemaVersion");
            Revision = (long)element.GetValue("Revision");

            foreach (var query in element.GetElement("Tables").ChildElements)
            {
                Tables.Add(new MetadataSchemaTables(query));
            }
            element.ErrorIfNotHandled();
        }

        public override void Save(SttpMarkupWriter writer)
        {
            writer.WriteValue("SchemaVersion", SchemaVersion);
            writer.WriteValue("Revision", Revision);
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