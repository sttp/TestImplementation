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

        public void Load(PayloadReader reader)
        {
            IsUpdateQuery = reader.ReadBoolean();
            UpdatedFromRevision = reader.ReadInt64();
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            TableName = reader.ReadString();
            Columns = reader.ReadListMetadataColumn();
        }
    }
}