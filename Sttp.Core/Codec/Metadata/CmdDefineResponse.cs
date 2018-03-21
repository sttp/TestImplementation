using System;
using System.Collections.Generic;

namespace Sttp.Codec.Metadata
{
    public class CmdDefineResponse
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.DefineResponse;

        public Guid? SchemaVersion;
        public long? SequenceNumber;
        public long? UpdatesSinceSequenceNumber;
        public string TableName;
        public List<MetadataColumn> Columns;

        public void Load(SttpMarkupReader reader)
        {
            var element = reader.ReadEntireElement();

            SchemaVersion = (Guid?)element.GetValue("SchemaVersion");
            SequenceNumber = (long?)element.GetValue("DataVersion");
            UpdatesSinceSequenceNumber = (long?)element.GetValue("UpdatesSinceSequenceNumber");
            TableName = (string)element.GetValue("TableName");
            Columns = new List<MetadataColumn>();

            foreach (var e in element.GetElement("Columns").ChildElements)
            {
                Columns.Add(new MetadataColumn(e));
            }
            element.ErrorIfNotHandled();
        }
    }
}