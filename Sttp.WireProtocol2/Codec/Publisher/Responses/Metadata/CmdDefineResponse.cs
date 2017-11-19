﻿using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.GetMetadataResponse
{
    public class CmdDefineResponse : ICmd
    {
        public SubCommand SubCommand => SubCommand.DefineResponse;

        public bool IsUpdateQuery;
        public Guid SchemaVersion;
        public long Revision;
        public long UpdatedFromRevision;
        public string TableName;
        public List<Tuple<string, SttpValueTypeCode>> Columns;

        public void Load(PacketReader reader)
        {
            IsUpdateQuery = reader.ReadBoolean();
            UpdatedFromRevision = reader.ReadInt64();
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
            TableName = reader.ReadString();
            Columns = reader.ReadList<string, SttpValueTypeCode>();
        }
    }
}