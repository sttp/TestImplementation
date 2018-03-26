using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandMetadataSchema : CommandBase
    {
        public readonly Guid SchemaVersion;
        public readonly long SequenceNumber;
        public readonly List<MetadataSchemaTable> Tables;

        public CommandMetadataSchema(Guid schemaVersion, long sequenceNumber, List<MetadataSchemaTable> tables)
            : base("MetadataSchema")
        {
            SchemaVersion = schemaVersion;
            SequenceNumber = sequenceNumber;
            Tables = new List<MetadataSchemaTable>(tables);
        }

        public CommandMetadataSchema(SttpMarkupReader reader)
            : base("MetadataSchema")
        {
            Tables = new List<MetadataSchemaTable>();
            var element = reader.ReadEntireElement();

            SchemaVersion = (Guid)element.GetValue("SchemaVersion");
            SequenceNumber = (long)element.GetValue("SequenceNumber");

            foreach (var query in element.GetElement("Tables").ChildElements)
            {
                Tables.Add(new MetadataSchemaTable(query));
            }
            element.ErrorIfNotHandled();
        }

        public CommandMetadataSchema Combine(CommandMetadataSchemaUpdate update)
        {
            List<MetadataSchemaTable> newList = new List<MetadataSchemaTable>(Tables);

            //Only support patching if the schemas match and the sequence number is less than the update.
            if (SchemaVersion == update.SchemaVersion && SequenceNumber < update.SequenceNumber)
            {
                foreach (var item in update.Tables)
                {
                    int indx = Tables.FindIndex(x => x.TableName == item.TableName);
                    newList[indx] = newList[indx].Clone(item.LastModifiedSequenceNumber);
                }
                return new CommandMetadataSchema(update.SchemaVersion, update.SequenceNumber, newList);
            }
            return this;
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