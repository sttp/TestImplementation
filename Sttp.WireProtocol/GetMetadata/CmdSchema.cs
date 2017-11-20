using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.GetMetadata
{
    public class CmdSchema : ICmd
    {
        public SubCommand SubCommand => SubCommand.Schema;

        public Guid SchemaVersion;
        public long Revision;

        public void Load(PayloadReader reader)
        {
            SchemaVersion = reader.ReadGuid();
            Revision = reader.ReadInt64();
        }
    }
}