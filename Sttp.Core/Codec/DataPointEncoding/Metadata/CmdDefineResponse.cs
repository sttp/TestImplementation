using System;
using System.Collections.Generic;

namespace Sttp.Codec.Metadata
{
    public class CmdDefineResponse
    {
        public MetadataSubCommand SubCommand => MetadataSubCommand.DefineResponse;

        public bool IsUpdateQuery;
        public Guid SchemaVersion;
        public long Revision;
        public long UpdatedFromRevision;
        public string TableName;
        public List<MetadataColumn> Columns;

        public void Load(SttpMarkupReader reader)
        {
            var element = reader.ReadEntireElement();
            if (element.ElementName != "DefineResponse")
                throw new Exception("Invalid command");

            IsUpdateQuery = (bool)element.GetValue("IsUpdateQuery");
            SchemaVersion = (Guid)element.GetValue("SchemaVersion");
            Revision = (long)element.GetValue("Revision");
            UpdatedFromRevision = (long)element.GetValue("UpdatedFromRevision");
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