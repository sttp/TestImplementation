using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;

namespace Sttp.Codec
{
    public class CommandMetadataSchema : DocumentCommandBase
    {
        public readonly Guid RuntimeID;
        public readonly long VersionNumber;
        public readonly List<MetadataSchemaTable> Tables;

        public CommandMetadataSchema(Guid runtimeID, long versionNumber, List<MetadataSchemaTable> tables)
            : base("MetadataSchema")
        {
            RuntimeID = runtimeID;
            VersionNumber = versionNumber;
            Tables = new List<MetadataSchemaTable>(tables);
        }

        public CommandMetadataSchema(CtpDocumentReader reader)
            : base("MetadataSchema")
        {
            Tables = new List<MetadataSchemaTable>();
            var element = reader.ReadEntireElement();

            RuntimeID = (Guid)element.GetValue("RuntimeID");
            VersionNumber = (long)element.GetValue("VersionNumber");

            foreach (var query in element.GetElement("Tables").ChildElements)
            {
                Tables.Add(new MetadataSchemaTable(query));
            }
            element.ErrorIfNotHandled();
        }

        public override void Save(CtpDocumentWriter writer)
        {
            writer.WriteValue("RuntimeID", RuntimeID);
            writer.WriteValue("VersionNumber", VersionNumber);
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

        public CommandMetadataSchema Combine(CommandMetadataSchemaUpdate update)
        {
            List<MetadataSchemaTable> newList = new List<MetadataSchemaTable>(Tables);

            //Only support patching if the schemas match and the sequence number is less than the update.
            if (RuntimeID == update.RuntimeID && VersionNumber < update.VersionNumber)
            {
                foreach (var item in update.Tables)
                {
                    int indx = Tables.FindIndex(x => x.TableName == item.TableName);
                    newList[indx] = newList[indx].Clone(item.LastModifiedVersionNumber);
                }
                return new CommandMetadataSchema(update.RuntimeID, update.VersionNumber, newList);
            }
            return this;
        }
    }
}