using System;
using System.Collections.Generic;

namespace Sttp.Codec.Metadata
{
    public class CmdDefineResponse
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.DefineResponse;

        public bool IsUpdateQuery;
        public Guid SchemaVersion;
        public long DataVersion;
        public long UpdatesSinceDataVersion;
        public string TableName;
        public List<MetadataColumn> Columns;

        public void Load(SttpMarkupReader reader)
        {
            var element = reader.ReadEntireElement();

            IsUpdateQuery = (bool)element.GetValue("IsUpdateQuery");
            SchemaVersion = (Guid)element.GetValue("SchemaVersion");
            DataVersion = (long)element.GetValue("DataVersion");
            UpdatesSinceDataVersion = (long)element.GetValue("UpdatesSinceDataVersion");
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